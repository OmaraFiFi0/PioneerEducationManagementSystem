using Pioneer_Education.Shared.DTOs.CategoryDTOs;
using Pioneer_Education.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.Abstraction
{
    public interface ICategoryService
    {
        Task<GenericResponse<IEnumerable<CategoryDTO>>> GetAllCategoryAsync(string? sort);

        Task<GenericResponse<CategoryDTO>> GetCategoryByIdAsync(int categoryId);

        Task<GenericResponse<bool>> CreateCategoryAsync(CreateCategoryDTO createCategory);

        Task<GenericResponse<bool>> UpdateCategoryAsync(int categoryId, CreateCategoryDTO updateCategory);

        Task<GenericResponse<bool>> DeleteCategoryAsync(int categoryId);
    }
}
