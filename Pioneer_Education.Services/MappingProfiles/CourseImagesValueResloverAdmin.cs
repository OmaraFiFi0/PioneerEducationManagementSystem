using AutoMapper;
using AutoMapper.Execution;
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
    internal class CourseImagesValueResloverAdmin : IValueResolver<Course, CourseToUpdateDTO, List<string>>
    {
        private readonly IConfiguration _configuration;

        public CourseImagesValueResloverAdmin(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<string> Resolve(Course source, CourseToUpdateDTO destination, List<string> destMember, ResolutionContext context)
        {
            return source.CourseImages.Select(CA => $"{_configuration["URLs:BaseUrl"]}/Images/courses/{CA.PictureUrl}").ToList();
        }
    }
}
