using Microsoft.AspNetCore.Mvc;
using Pioneer_Education.Services.Abstraction;
using Pioneer_Education.Services.Services;
using Pioneer_Education.Shared.DTOs.CategoryDTOs;
using Pioneer_Education.Shared.DTOs.CourseDTOs;
using Pioneer_Education.Shared.Responses;

namespace Pioneer_Education.API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET : BaseUrl/api/Category
        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<CategoryDTO>>>> GetAllCourses([FromQuery] string? sort)
        {
            var result = await _categoryService.GetAllCategoryAsync(sort);
            return HandleResult(result);
        }

        // GET : BaseUrl/apu/Category/Id

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<GenericResponse<CategoryDTO>>> GetCategory([FromRoute] int categoryId)
        {
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);
            return HandleResult(result);
        }

        // POST : BaseUrl/api/Category
        [HttpPost]
        public async Task<ActionResult<GenericResponse<bool>>> CreateCategory([FromBody] CreateCategoryDTO createCategory)
        {
            var result = await _categoryService.CreateCategoryAsync(createCategory);
            return HandleResult(result);
        }
        // PUT : BaseUrl/api/Category/categoryId
        [HttpPut("{categoryId}")]
        public async Task<ActionResult<GenericResponse<bool>>> UpdateCategory([FromRoute] int categoryId, [FromBody] CreateCategoryDTO updateCategory)
        {
            var result = await _categoryService.UpdateCategoryAsync(categoryId, updateCategory);
            return HandleResult(result);
        }

        // Delete : BaseUrl/api/Category/categoryId
        [HttpDelete("{courseId}")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteCourseAsync([FromRoute] int courseId)
        {
            var result = await _categoryService.DeleteCategoryAsync(courseId);
            return HandleResult(result);
        }

    }
}
