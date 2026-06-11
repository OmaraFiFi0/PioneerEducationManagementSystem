using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Shared.DTOs.CourseDTOs
{
    public class CourseDetailsDTO
    {
        public string CourseName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }


        //public int EnrolledStudents { get; set; }

        //public int FreeSpaceInCourse { get; set; }

        public string CourseLevel { get; set; } = null!;

        public string CourseStatus { get; set; } = null!;

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public string CategoryName { get; set; } = null!;

        public List<string> ImageUrls { get; set; } = [];



    }
}
