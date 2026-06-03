using Microsoft.Extensions.Logging;
using Pioneer_Education.Core.Contracts;
using Pioneer_Education.Core.Entities.CouresModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Infrastructure.Jobs
{
    public class CourseStatusJob
    {
        private readonly IUnitOfWork _unitOfWork;

        public CourseStatusJob(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UpdateCourseStatusAsync()
        {
            var TodayDate = DateTime.Now;

            var courses = await _unitOfWork
                .GetRepository<Course, int>()
                .GetAllAsync(c =>
         (c.CourseStatus == CourseStatus.Scheduled && c.StartDate != null && c.StartDate <= TodayDate)
                                                         ||
        (c.CourseStatus == CourseStatus.Published && c.EndDate != null && c.EndDate <= TodayDate));
            foreach (var course in courses)
            {
                // Scheduled -> Published

                if (course.CourseStatus == CourseStatus.Scheduled &&
                    course.StartDate is not null &&
                    course.StartDate <= TodayDate)
                {
                    course.IsActive = true;
                    course.CourseStatus = CourseStatus.Published;
                }
                // Published -> Closed

                if (course.CourseStatus == CourseStatus.Published
                    && course.EndDate is not null
                    && course.EndDate <= TodayDate)
                {
                    course.IsActive = false;
                    course.CourseStatus = CourseStatus.Closed;
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }




    }



}



