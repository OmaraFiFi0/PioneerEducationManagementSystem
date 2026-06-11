using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pioneer_Education.Core.Contracts;
using Pioneer_Education.Core.Entities.CategoryModule;
using Pioneer_Education.Core.Entities.CouresModule;
using Pioneer_Education.Services.Abstraction;
using Pioneer_Education.Shared.DTOs.CourseDTOs;
using Pioneer_Education.Shared.QueryParameters;
using Pioneer_Education.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CourseService> _logger;
        private readonly IAttacehmentService _attacehmentService;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CourseService> logger, IAttacehmentService attacehmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _attacehmentService = attacehmentService;
        }



        public async Task<GenericResponse<IEnumerable<CourseDTO>>> GetAllCoursesForStudentAsync(string? courseLevel, string? sort)
        {

            var genericResponse = new GenericResponse<IEnumerable<CourseDTO>>();
            Enum.TryParse(courseLevel, out CourseLevel courseLevelEnum);
            Expression<Func<Course, bool>> filter = C =>
                            (courseLevel == null || C.CourseLevel == courseLevelEnum)
                      && (C.CourseStatus != CourseStatus.Draft)
                      && (C.Category.IsActive != false);


            Expression<Func<Course, object>>? OrderBy = null!;
            Expression<Func<Course, object>>? OrderByDescending = null!;

            if (sort is not null)
            {
                switch (sort)
                {
                    case "PriceAsc":
                        OrderBy = C => C.Price;
                        break;
                    case "PriceDesc":
                        OrderByDescending = C => C.Price;
                        break;
                    default:
                        OrderBy = C => C.Id;
                        break;

                }
            }
            else
            {
                OrderBy = C => C.Id;
            }


            var courses = await _unitOfWork.GetRepository<Course, int>()
                             .GetAllAsync(filter, OrderBy, OrderByDescending, [X => X.CourseImages, X => X.Category]);

            if (courses is null || !courses.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status404NotFound;
                genericResponse.Message = "No Courses Found";

                return genericResponse;
            }



            var mappedCourses = _mapper.Map<IEnumerable<Course>, IEnumerable<CourseDTO>>(courses);

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Courses Retrived Successfully";
            genericResponse.Data = mappedCourses;

            return genericResponse;
        }

        public async Task<GenericResponse<CourseDetailsDTO>> GetCourseDetailsAsync(int courseId)
        {
            var genericResponse = new GenericResponse<CourseDetailsDTO>();


            Expression<Func<Course, bool>> filter = C =>
                 C.CourseStatus != CourseStatus.Draft && C.Category.IsActive != false;


            var course = await _unitOfWork.GetRepository<Course, int>()
                .GetByIdAsync(courseId, filter, [I => I.CourseImages, C => C.Category]);

            if (course is null)
            {
                genericResponse.StatusCode = StatusCodes.Status404NotFound;
                genericResponse.Message = "Course Not Found";

                return genericResponse;
            }

            var mappedRoom = _mapper.Map<CourseDetailsDTO>(course);

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Course Details Retrieved Successfully.";
            genericResponse.Data = mappedRoom;

            return genericResponse;

        }

        public async Task<GenericResponse<IEnumerable<CourseForAdminDTO>>> GetAllCoursesForAdminOrSttafAsync(CourseQueryParams? queryParams)
        {
            var genericResponse = new GenericResponse<IEnumerable<CourseForAdminDTO>>();
            IEnumerable<Course>? courses = null!;
            if (queryParams != null)
            {
                Enum.TryParse(queryParams.courseLevel, out CourseLevel courseLevelEnum);
                Enum.TryParse(queryParams.courseStatus, out CourseStatus courseStatusEnum);
                Expression<Func<Course, bool>> filter = C =>
                                                        (queryParams.courseLevel == null || C.CourseLevel == courseLevelEnum)
                                                    && (queryParams.courseStatus == null || C.CourseStatus == courseStatusEnum);
                Expression<Func<Course, object>>? orderByExp = null;
                Expression<Func<Course, object>>? orderByDescendingExp = null!;

                if (queryParams.sort is not null)
                {
                    switch (queryParams.sort)
                    {
                        case "PriceAsc":
                            orderByExp = C => C.Price;
                            break;
                        case "PriceDesc":
                            orderByDescendingExp = C => C.Price;
                            break;
                        case "StartDateAsc":
                            orderByExp = C => C.StartDate!;
                            break;

                        case "StartDateDesc":
                            orderByDescendingExp = C => C.StartDate!;
                            break;
                        case "EndDateAsc":
                            orderByExp = C => C.EndDate!;
                            break;
                        case "EndDateDesc":
                            orderByDescendingExp = C => C.EndDate!;
                            break;
                        default:
                            orderByExp = C => C.Id;
                            break;
                    }
                }
                else
                {
                    orderByExp = C => C.Id;
                }


                courses = await _unitOfWork.GetRepository<Course, int>()
                   .GetAllAsync(filter, orderByExp, orderByDescendingExp, [C => C.Category]);


            }
            else
            {
                courses = await _unitOfWork.GetRepository<Course, int>().GetAllAsync();
            }


            if (courses is null || !courses.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status404NotFound;
                genericResponse.Message = "No Courses  Avaliable";

                return genericResponse;

            }

            var mappedCourse = _mapper.Map<IEnumerable<CourseForAdminDTO>>(courses);
            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Rooms Retrived SuccessFully";
            genericResponse.Data = mappedCourse;
            return genericResponse;
        }

        public async Task<GenericResponse<bool>> CreateCourseAsync(CourseToCreateDTO createCourse)
        {
            var genericResponse = new GenericResponse<bool>();

            try
            {
                if (createCourse is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = " Invalid Course Data";

                    return genericResponse;
                }

                var categoryExists = await _unitOfWork.GetRepository<Category, int>().GetAllAsync
                    (
                    X => X.Id == createCourse.CategoryId
                    && X.IsActive
                    );

                if (!categoryExists.Any())
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = "Category Not Found Or Inactive";

                    return genericResponse;
                }

                var courseToCreated = _mapper.Map<CourseToCreateDTO, Course>(createCourse); // DeAttached 

                switch (courseToCreated.CourseStatus)
                {
                    case CourseStatus.Draft:
                        courseToCreated.IsActive = false;
                        courseToCreated.StartDate = null;
                        courseToCreated.EndDate = null;
                        break;
                    case CourseStatus.Published:
                        courseToCreated.IsActive = true;
                        if (courseToCreated.StartDate is null || courseToCreated.StartDate > DateTime.Now)
                        {
                            genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                            genericResponse.Message = "Published course start date must be in the past";

                            return genericResponse;
                        }
                        if (courseToCreated.EndDate is null || courseToCreated.EndDate < DateTime.Now)
                        {
                            genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                            genericResponse.Message = "Published course end date must be in the future.";

                            return genericResponse;
                        }
                        break;
                    case CourseStatus.Scheduled:
                        courseToCreated.IsActive = false;
                        if (courseToCreated.StartDate is null || courseToCreated.StartDate <= DateTime.Now)
                        {
                            genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                            genericResponse.Message = " Scheduled course start date must be in the future.";

                            return genericResponse;
                        }
                        if (courseToCreated.EndDate is null || courseToCreated.EndDate <= DateTime.Now)
                        {
                            genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                            genericResponse.Message = " Scheduled course end date must be in the future.";

                            return genericResponse;
                        }
                        break;
                    case CourseStatus.Closed:
                        courseToCreated.FreeSpaceInCourse = courseToCreated.Capacity - courseToCreated.EnrolledStudents;
                        bool isFull = courseToCreated.FreeSpaceInCourse == 0;
                        bool isEnded = courseToCreated.EndDate <= DateTime.Now;
                        if (isFull || isEnded)
                        {
                            courseToCreated.IsActive = false;
                        }
                        else
                        {
                            if (courseToCreated.StartDate <= DateTime.Now)
                            {
                                courseToCreated.IsActive = true;
                                courseToCreated.CourseStatus = CourseStatus.Published;
                            }
                            else if (courseToCreated.StartDate > DateTime.Now)
                            {
                                courseToCreated.IsActive = false;
                                courseToCreated.CourseStatus = CourseStatus.Scheduled;
                            }
                        }
                        break;
                    case CourseStatus.Archived:
                        courseToCreated.IsActive = false;
                        break;

                }
                courseToCreated.EnrolledStudents = 0;

                courseToCreated.FreeSpaceInCourse = courseToCreated.Capacity - courseToCreated.EnrolledStudents;

                await _unitOfWork.GetRepository<Course, int>().AddAsync(courseToCreated); //Added 

                var result = await _unitOfWork.SaveChangesAsync() > 0;
                if (result)
                {

                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = " Course Created SuccessFully";
                    genericResponse.Data = true;
                }
                else
                {
                    genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                    genericResponse.Message = " Failed To Create Course";
                }
                return genericResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " An unExpected Error Occurred While Create Course");
                genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                genericResponse.Message = " An UnExpected Error Occurred";
                return genericResponse;
            }
        }

        public async Task<GenericResponse<CourseToUpdateDTO>> GetCourseDataForAdminToUpdateAsync(int courseId)
        {
            var genericResponse = new GenericResponse<CourseToUpdateDTO>();

            var course = await _unitOfWork.GetRepository<Course, int>().GetByIdAsync(courseId, null, [I => I.CourseImages, C => C.Category]);

            if (course is null)
            {
                genericResponse.StatusCode = StatusCodes.Status404NotFound;
                genericResponse.Message = " Course Not Found ";

                return genericResponse;
            }


            var mappedCourse = _mapper.Map<Course, CourseToUpdateDTO>(course);
            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = " Course Retrived Successfully";
            genericResponse.Data = mappedCourse;

            return genericResponse;
        }

        public async Task<GenericResponse<bool>> UpdateCourseAsync(int courseId, CourseToUpdateDTO updateCourse)
        {
            var genericResponse = new GenericResponse<bool>();

            try
            {
                var course = await _unitOfWork.GetRepository<Course, int>().GetByIdAsync(courseId);

                if (course is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status404NotFound;
                    genericResponse.Message = " Course Not Found";

                    return genericResponse;
                }

                var categoryExists = await _unitOfWork.GetRepository<Category, int>().GetByIdAsync(updateCourse.categoryId);

                if (categoryExists is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status404NotFound;
                    genericResponse.Message = "Category Not Found";
                    return genericResponse;
                }

                else if (!categoryExists.IsActive)
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = "Category Is Inactive";
                    return genericResponse;
                }

                _mapper.Map(updateCourse, course);


                switch (course.CourseStatus)
                {
                    case CourseStatus.Draft:
                        course.IsActive = false;
                        course.StartDate = null;
                        course.EndDate = null;
                        break;
                    case CourseStatus.Published:
                        course.IsActive = true;
                        if (course.StartDate is null || course.StartDate > DateTime.Now)
                        {
                            genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                            genericResponse.Message = "Published course start date must be in the past";

                            return genericResponse;
                        }
                        if (course.EndDate is null || course.EndDate < DateTime.Now)
                        {
                            genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                            genericResponse.Message = "Published course end date must be in the future.";

                            return genericResponse;
                        }
                        break;
                    case CourseStatus.Scheduled:
                        course.IsActive = false;
                        if (course.StartDate is null || course.StartDate <= DateTime.Now)
                        {
                            genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                            genericResponse.Message = " Scheduled course start date must be in the future.";

                            return genericResponse;
                        }
                        if (updateCourse.EndDate is null || updateCourse.EndDate <= DateTime.Now)
                        {
                            genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                            genericResponse.Message = " Scheduled course end date must be in the future.";

                            return genericResponse;
                        }
                        break;
                    case CourseStatus.Closed:
                        course.FreeSpaceInCourse = course.Capacity - course.EnrolledStudents;
                        bool isFull = course.FreeSpaceInCourse == 0;
                        bool isEnded = course.EndDate <= DateTime.Now;
                        if (isFull || isEnded)
                        {
                            Console.WriteLine($"The Free Space is {course.FreeSpaceInCourse} || {!isEnded}");
                            course.IsActive = false;
                        }
                        else
                        {
                            if (course.StartDate <= DateTime.Now)
                            {
                                course.IsActive = true;
                                course.CourseStatus = CourseStatus.Published;
                            }
                            else if (course.StartDate > DateTime.Now)
                            {
                                course.IsActive = false;
                                course.CourseStatus = CourseStatus.Scheduled;
                            }
                        }
                        break;
                    case CourseStatus.Archived:
                        course.IsActive = false;
                        break;
                }

                _unitOfWork.GetRepository<Course, int>().Update(course); // Marked As Modified 
                course.UpdatedAt = DateTime.Now;
                var result = await _unitOfWork.SaveChangesAsync() > 0;
                if (result)
                {
                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = "Update Course Successfully";
                    genericResponse.Data = true;
                }
                else
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = "Failed To Update Course";

                }
                return genericResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Unexpected Error Occurred While Update Course");
                genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                genericResponse.Message = "An Unexpected Error Occurred While Update Course";
                return genericResponse;
            }
        }

        public async Task<GenericResponse<bool>> DeleteCourseAsync(int courseId)
        {
            var genericResponse = new GenericResponse<bool>();

            try
            {
                var course = await _unitOfWork.GetRepository<Course, int>().GetByIdAsync(courseId);

                if (course is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status404NotFound;
                    genericResponse.Message = " No Course Avaliable To Delete ";

                    return genericResponse;
                }

                course.CourseStatus = CourseStatus.Archived;
                _unitOfWork.GetRepository<Course, int>().Update(course); // Marked As Modified
                course.UpdatedAt = DateTime.Now;
                var result = await _unitOfWork.SaveChangesAsync() > 0;
                if (result)
                {
                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = "Success To Delete Course";
                    genericResponse.Data = true;
                }
                else
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = "Failed To Delete Course";
                }
                return genericResponse;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An UnExpected Error Occurred While Deleting Course");
                genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                genericResponse.Message = "Failed To Delete Course";
                return genericResponse;
            }

        }

        public async Task<GenericResponse<bool>> UploadCourseImagesAsync(int courseId, List<IFormFile> files)
        {
            var genericResponse = new GenericResponse<bool>();

            try
            {
                var course = await _unitOfWork.GetRepository<Course, int>().GetByIdAsync(courseId);

                if (course is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status404NotFound;
                    genericResponse.Message = $" No Course Found To Upload Images With This Id :{courseId}";
                    return genericResponse;
                }

                // To Upload Files To Server
                foreach (var file in files)
                {
                    var fileName = await _attacehmentService.UploadFileAsync(file, "courses");

                    if (fileName is null) continue;

                    var courseImage = new CourseImage
                    {
                        CourseId = course.Id,
                        PictureUrl = fileName,
                    };
                    await _unitOfWork.GetRepository<CourseImage, int>().AddAsync(courseImage);
                }
                var result = await _unitOfWork.SaveChangesAsync() > 0;
                if (result)
                {
                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = "Success To Upload Course Images";
                    genericResponse.Data = true;
                }
                else
                {
                    genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                    genericResponse.Message = "Failed To Upload Course Images ";

                }
                return genericResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An UnExpected Error Occurred While Uploading Course Images ");
                genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                genericResponse.Message = "Failed To Upload Course Images ";
                return genericResponse;

            }
        }

        public async Task<GenericResponse<bool>> DeleteCourseImageAsync(int courseId, int imageId)
        {
            var genericResponse = new GenericResponse<bool>();
            try
            {
                var course = await _unitOfWork.GetRepository<Course, int>().GetByIdAsync(courseId, null, [C => C.CourseImages]);

                if (course is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status404NotFound;
                    genericResponse.Message = "Course Not Found To Delete It's Image";
                    return genericResponse;
                }
                if (course.CourseImages is null || course.CourseImages.Count == 0)
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = "No Images Found To This Course";
                    return genericResponse;
                }

                var courseImage = course.CourseImages.FirstOrDefault(CI => CI.Id == imageId);

                if (courseImage is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status404NotFound;
                    genericResponse.Message = "No Image With Id Found To Delete ";
                    return genericResponse;
                }


                _unitOfWork.GetRepository<CourseImage, int>().Delete(courseImage);
                var isDeletedFromServer = _attacehmentService.DeleteFile(courseImage.PictureUrl, "courses");
                if (!isDeletedFromServer)
                {
                    genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                    genericResponse.Message = " Failed To Delete This Image";
                    return genericResponse;
                }
                var result = await _unitOfWork.SaveChangesAsync() > 0;

                if (result)
                {
                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = " Success To Delete This Image";
                    genericResponse.Data = true;
                }
                else
                {
                    genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                    genericResponse.Message = " Failed To Delete This Image";
                }
                return genericResponse;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An UnExpected Error Occurred While Deleting Image");
                genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                genericResponse.Message = " Failed To Delete This Image";
                return genericResponse;
            }

        }
    }
}
