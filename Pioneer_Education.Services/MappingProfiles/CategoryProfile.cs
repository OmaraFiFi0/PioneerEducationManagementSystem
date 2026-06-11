using AutoMapper;
using Pioneer_Education.Core.Entities.CategoryModule;
using Pioneer_Education.Shared.DTOs.CategoryDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id));


            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Category_Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Category_Description))
                .ReverseMap();



        }
    }
}
