using Microsoft.AspNetCore.Http;
using Pioneer_Education.Shared.DTOs.CourseDTOs;
using Pioneer_Education.Shared.QueryParameters;
using Pioneer_Education.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.Abstraction
{
    public interface ICourseService
    {
        Task<GenericResponse<IEnumerable<CourseDTO>>> GetAllCoursesForStudentAsync(string? courseLevel, string? sort);

        Task<GenericResponse<CourseDetailsDTO>> GetCourseDetailsAsync(int courseId);

        Task<GenericResponse<IEnumerable<CourseForAdminDTO>>> GetAllCoursesForAdminOrSttafAsync(CourseQueryParams? queryParams);

        Task<GenericResponse<bool>> CreateCourseAsync(CourseToCreateDTO createCourse);

        Task<GenericResponse<CourseToUpdateDTO>> GetCourseDataForAdminToUpdateAsync(int courseId);

        Task<GenericResponse<bool>> UpdateCourseAsync(int courseId, CourseToUpdateDTO updateCourse);

        Task<GenericResponse<bool>> DeleteCourseAsync(int courseId);

        Task<GenericResponse<bool>> UploadCourseImagesAsync(int courseId, List<IFormFile> files);

        Task<GenericResponse<bool>> DeleteCourseImageAsync(int courseId, int imageId);
    }
}
