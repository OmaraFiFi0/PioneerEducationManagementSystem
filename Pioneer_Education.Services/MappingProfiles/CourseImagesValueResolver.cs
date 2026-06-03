using AutoMapper;
using Microsoft.Extensions.Configuration;
using Pioneer_Education.Core.Entities.CouresModule;
using Pioneer_Education.Shared.DTOs.CourseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Services.MappingProfiles
{
    internal class CourseImagesValueResolver : IValueResolver<Course, CourseDetailsDTO, List<string>>
    {
        private readonly IConfiguration _configuration;

        public CourseImagesValueResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<string> Resolve(Course source, CourseDetailsDTO destination, List<string> destMember, ResolutionContext context)
        {
            return source.CourseImages.Select(C => $"{_configuration["URLs:BaseUrl"]}/Images/courses/{C.PictureUrl}").ToList();
        }
    }
}
