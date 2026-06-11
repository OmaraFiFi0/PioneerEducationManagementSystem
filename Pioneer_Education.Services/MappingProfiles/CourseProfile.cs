using AutoMapper;
using Pioneer_Education.Core.Entities.CouresModule;
using Pioneer_Education.Shared.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.MappingProfiles
{
    internal class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDTO>()
                .ForMember(dest => dest.CategoryName,
                               opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Course, CourseDetailsDTO>()
                .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom<CourseImagesValueResolver>())
                .ForMember(dest => dest.CategoryName,
                               opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<Course, CourseForAdminDTO>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<CourseToCreateDTO, Course>();

            CreateMap<Course, CourseToUpdateDTO>()
                .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom<CourseImagesValueResloverAdmin>())
                .ForMember(dest => dest.categoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.categoryDescription, opt => opt.MapFrom(src => src.Category.Description))
                .ReverseMap();

        }
    }
}
