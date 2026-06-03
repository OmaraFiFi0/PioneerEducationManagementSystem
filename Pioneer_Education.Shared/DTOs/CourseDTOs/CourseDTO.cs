using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Shared.DTOs.CourseDTOs
{
    public class CourseDTO
    {
        public int Id { get; set; }

        public string CourseName { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string CourseLevel { get; set; } = null!;

        public decimal Price { get; set; }

        public int DurationInHours { get; set; }

        public string CourseStatus { get; set; } = null!;
    }
}
