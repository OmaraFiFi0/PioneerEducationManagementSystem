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
            CreateMap<Course, CourseDTO>();

            CreateMap<Course, CourseDetailsDTO>()
                .ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom<CourseImagesValueResolver>());

            CreateMap<Course, CourseForAdminDTO>();

            CreateMap<CourseToCreateDTO, Course>();

            CreateMap<Course, CourseToUpdateDTO>().ForMember(dest => dest.ImageUrls,
                opt => opt.MapFrom<CourseImagesValueResloverAdmin>()).ReverseMap();

        }
    }
}
