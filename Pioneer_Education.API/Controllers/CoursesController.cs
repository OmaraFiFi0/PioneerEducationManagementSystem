using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pioneer_Education.Infrastructure.Repository;
using Pioneer_Education.Services.Abstraction;
using Pioneer_Education.Shared.DTOs.CourseDTOs;
using Pioneer_Education.Shared.QueryParameters;
using Pioneer_Education.Shared.Responses;

namespace Pioneer_Education.API.Controllers
{

    public class CoursesController : BaseApiController
    {
        private readonly ICourseService _courseService;


        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // GET: BaseUrl/api/Courses
        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<CourseDTO>>>> GetAllCourses([FromQuery] string? courseLevel, [FromQuery] string? sort)
        {
            var result = await _courseService.GetAllCoursesForStudentAsync(courseLevel, sort);
            return HandleResult(result);
        }
        // GET: BaseUrl/api/Courses/{id}
        [HttpGet("{id}")]

        public async Task<ActionResult<GenericResponse<CourseDetailsDTO>>> GetCourseDetails([FromRoute] int id)
        {
            var result = await _courseService.GetCourseDetailsAsync(id);
            return HandleResult(result);
        }
        // Get All Course Data For Admin or Instructor To Update IT 
        // GET: BaseUrl/api/Courses/Admin
        [HttpGet("Admin")]
        public async Task<ActionResult<GenericResponse<IEnumerable<CourseForAdminDTO>>>> GetCoursesForAdmin([FromQuery] CourseQueryParams? queryParams)
        {
            var result = await _courseService.GetAllCoursesForAdminOrSttafAsync(queryParams);
            return HandleResult(result);
        }
        // POST : BaseUrl/api/Courses
        [HttpPost]
        public async Task<ActionResult<GenericResponse<bool>>> CreateCourse([FromBody] CourseToCreateDTO createCourse)
        {
            var result = await _courseService.CreateCourseAsync(createCourse);
            return HandleResult(result);
        }

        // GetData For Admin or Instructor To Update IT 
        // GET : BaseUrl/api/Courses/{id}/GetData
        [HttpGet("{courseId}/GetData")]
        public async Task<ActionResult<GenericResponse<CourseToUpdateDTO>>> GetCourseDataForAdminToUpdate(int courseId)
        {
            var result = await _courseService.GetCourseDataForAdminToUpdateAsync(courseId);
            return HandleResult(result);
        }

        // PUT : BaseUrl/api/Courses/{id}
        [HttpPut("{courseId}")]
        public async Task<ActionResult<GenericResponse<bool>>> UpdateCourseAsync([FromRoute] int courseId, [FromForm] CourseToUpdateDTO updateCourse)
        {
            var result = await _courseService.UpdateCourseAsync(courseId, updateCourse);
            return HandleResult(result);
        }

        // Delete : BaseUrl/api/Courses
        [HttpDelete("{courseId}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteCourseAsync([FromRoute] int courseId)
        {
            var result = await _courseService.DeleteCourseAsync(courseId);
            return HandleResult(result);
        }

        // POST : BaseUrl/api/Courses/{courseId}/Images
        [HttpPost("{courseId}/Images")]
        public async Task<ActionResult<GenericResponse<bool>>> UploadCourseImages([FromRoute] int courseId, [FromForm] List<IFormFile> files)
        {
            var result = await _courseService.UploadCourseImagesAsync(courseId, files);
            return HandleResult(result);
        }

        // Delete : BaseUrl/api/Courses/Courseid/Images/ImageId
        [HttpDelete("{courseId}/Images/{imageId}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteImage([FromRoute] int courseId, [FromRoute] int imageId)
        {
            var result = await _courseService.DeleteCourseImageAsync(courseId, imageId);
            return HandleResult(result);
        }
    }
}
