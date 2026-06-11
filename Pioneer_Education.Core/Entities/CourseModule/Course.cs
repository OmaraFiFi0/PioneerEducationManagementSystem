using Pioneer_Education.Core.Entities.CategoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Core.Entities.CouresModule
{
    public class Course : BaseEntity<int>
    {
        public string CourseName { get; set; } = default!;

        public string Description { get; set; } = default!;

        public int Capacity { get; set; }

        public int EnrolledStudents { get; set; }
        public int FreeSpaceInCourse { get; set; }
        public decimal Price { get; set; }
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } // IF You Wan't To Make Soft Delete To Course 

        public int DurationInHours { get; set; }

        public CourseLevel CourseLevel { get; set; }

        public CourseStatus CourseStatus { get; set; }

        public ICollection<CourseImage> CourseImages { get; set; } = [];

        /// <summary>
        /// ///// عاوز ارجعه ميقبلشي null بي سطر الـ SQL دي 
        /// UPDATE Course
        //// SET CategoryId = 1
        /// WHERE CategoryId IS NULL
        /// </summary>
        /// 
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

    }
}
