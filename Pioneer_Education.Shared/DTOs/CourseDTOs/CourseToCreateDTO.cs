using Pioneer_Education.Shared.DTOs.SharedEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Shared.DTOs.CourseDTOs
{
    public class CourseToCreateDTO
    {
        [Required(ErrorMessage = "Course Name Is Required")]
        [MaxLength(150, ErrorMessage = "Max Length To Course Name Is 150 Characters")]
        [MinLength(3, ErrorMessage = "Min Length To Course Name Is 3 Characters")]
        public string CourseName { get; set; } = null!;
        [Required(ErrorMessage = " Course Status Is Required ")]
        public CourseStatus CourseStatus { get; set; }
        [Required(ErrorMessage = "Course Level Is Required ")]
        public CourseLevel CourseLevel { get; set; }

        [Required(ErrorMessage = "Description Is Required")]
        [MaxLength(500, ErrorMessage = "Max Length To Description Is 500 Characters")]
        [MinLength(10, ErrorMessage = "Min Length To Description Is 10 Characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = " Capacity Is Required ")]
        [Range(0, 30, ErrorMessage = " The Maximun Students In Course Is 30 ")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "Price Is Required ")]
        [Range(0, double.MaxValue, ErrorMessage = " Price Must Be Positive Value ")]
        public decimal Price { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = " DurationInHours Is Required ")]
        [Range(0, 100, ErrorMessage = " The Maximun DurationInHours Is 100 Hours , Must Be Positive Value")]
        public int DurationInHours { get; set; }

        [Required(ErrorMessage = "CategoryId Is Required ")]
        public int CategoryId { get; set; }

    }
}
