using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pioneer_Education.Core.Contracts;
using Pioneer_Education.Core.Entities.CategoryModule;
using Pioneer_Education.Core.Entities.CouresModule;
using Pioneer_Education.Services.Abstraction;
using Pioneer_Education.Shared.DTOs.CategoryDTOs;
using Pioneer_Education.Shared.DTOs.CourseDTOs;
using Pioneer_Education.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }



        public async Task<GenericResponse<IEnumerable<CategoryDTO>>> GetAllCategoryAsync(string? sort)
        {
            var genericResponse = new GenericResponse<IEnumerable<CategoryDTO>>();

            Expression<Func<Category, object>>? OrderBy = null!;
            Expression<Func<Category, object>>? OrderByDescending = null!;
            if (sort is not null)
            {
                switch (sort)
                {
                    case "IdAsc":
                        OrderBy = C => C.Id;
                        break;
                    case "IdDesc":
                        OrderByDescending = C => C.Id;
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
            var categories = await _unitOfWork.GetRepository<Category, int>().GetAllAsync(null, OrderBy, OrderByDescending, null);

            if (categories is null || !categories.Any())
            {
                genericResponse.StatusCode = StatusCodes.Status404NotFound;
                genericResponse.Message = "No Categories Found";

                return genericResponse;
            }

            var mappedCourses = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(categories);

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Courses Retrived Successfully";
            genericResponse.Data = mappedCourses;

            return genericResponse;
        }

        public async Task<GenericResponse<CategoryDTO>> GetCategoryByIdAsync(int categoryId)
        {
            var genericResponse = new GenericResponse<CategoryDTO>();

            var category = await _unitOfWork.GetRepository<Category, int>().GetByIdAsync(categoryId);

            if (category is null)
            {
                genericResponse.StatusCode = StatusCodes.Status404NotFound;
                genericResponse.Message = "category Not Found";

                return genericResponse;
            }

            var mappedRoom = _mapper.Map<CategoryDTO>(category);

            genericResponse.StatusCode = StatusCodes.Status200OK;
            genericResponse.Message = "Course Details Retrieved Successfully.";
            genericResponse.Data = mappedRoom;

            return genericResponse;
        }

        public async Task<GenericResponse<bool>> CreateCategoryAsync(CreateCategoryDTO createCategory)
        {
            var genericResponse = new GenericResponse<bool>();

            try
            {
                if (createCategory is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = " Invalid Category Data";

                    return genericResponse;
                }

                var MappedCategory = _mapper.Map<Category>(createCategory);

                //var MappedCategory = new Category
                //{
                //    Name = createCategory.Category_Name,
                //    Description = createCategory.Category_Description
                //};


                await _unitOfWork.GetRepository<Category, int>().AddAsync(MappedCategory);

                var result = await _unitOfWork.SaveChangesAsync() > 0;


                if (result)
                {

                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = " Category Created SuccessFully";
                    genericResponse.Data = true;
                }
                else
                {
                    genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                    genericResponse.Message = " Failed To Create Category";
                }
                return genericResponse;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An UnExpected Error Occurred While Creating Category");
                genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                genericResponse.Message = "An UnExpected Error Occurred While Creating Category";
                return genericResponse;
            }



        }

        public async Task<GenericResponse<bool>> UpdateCategoryAsync(int categoryId, CreateCategoryDTO updateCategory)
        {
            var genericResponse = new GenericResponse<bool>();

            try
            {
                var category = await _unitOfWork.GetRepository<Category, int>().GetByIdAsync(categoryId);

                if (category is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status404NotFound;
                    genericResponse.Message = "Category Not Found";
                    return genericResponse;
                }

                var MappedCategory = _mapper.Map(updateCategory, category);

                _unitOfWork.GetRepository<Category, int>().Update(category);

                category.UpdatedAt = DateTime.Now;
                var result = await _unitOfWork.SaveChangesAsync() > 0;
                if (result)
                {
                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = "Update Category Successfully";
                    genericResponse.Data = true;
                }
                else
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = "Failed To Update Category";

                }
                return genericResponse;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An UnExpected Error Occurred While Updating Category");
                genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                genericResponse.Message = "An UnExpected Error Occurred While Updating Category";
                return genericResponse;
            }
        }

        public async Task<GenericResponse<bool>> DeleteCategoryAsync(int categoryId)
        {
            var genericResponse = new GenericResponse<bool>();

            try
            {
                var category = await _unitOfWork.GetRepository<Category, int>().GetByIdAsync(categoryId);

                if (category is null)
                {
                    genericResponse.StatusCode = StatusCodes.Status404NotFound;
                    genericResponse.Message = " No Category Avaliable To Delete ";
                    return genericResponse;
                }

                if (category.Courses != null && category.Courses.Any())
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = " Cannot Delete Category With Associated Courses";
                    return genericResponse;
                }

                category.IsActive = false;
                _unitOfWork.GetRepository<Category, int>().Update(category);
                category.UpdatedAt = DateTime.Now;

                var result = await _unitOfWork.SaveChangesAsync() > 0;
                if (result)
                {
                    genericResponse.StatusCode = StatusCodes.Status200OK;
                    genericResponse.Message = "Success To Delete Category";
                    genericResponse.Data = true;
                }
                else
                {
                    genericResponse.StatusCode = StatusCodes.Status400BadRequest;
                    genericResponse.Message = "Failed To Delete Category";
                }
                return genericResponse;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An UnExpected Error Occurred While Deleting Category");
                genericResponse.StatusCode = StatusCodes.Status500InternalServerError;
                genericResponse.Message = "Failed To Delete Category";
                return genericResponse;
            }

        }
    }
}
