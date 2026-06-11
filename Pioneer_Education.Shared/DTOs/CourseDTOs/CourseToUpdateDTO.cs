using Pioneer_Education.Shared.DTOs.SharedEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Shared.DTOs.CourseDTOs
{
    public class CourseToUpdateDTO
    {
        [Required(ErrorMessage = "Course Name Is Required")]
        [MaxLength(150, ErrorMessage = "Max Length To Course Name Is 150 Characters")]
        [MinLength(3, ErrorMessage = "Min Length To Course Name Is 3 Characters")]
        public string CourseName { get; set; } = null!;

        [Required(ErrorMessage = "Description Is Required")]
        [MaxLength(500, ErrorMessage = "Max Length To Description Is 500 Characters")]
        [MinLength(10, ErrorMessage = "Min Length To Description Is 10 Characters")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Course Level Is Required")]
        public string CourseLevel { get; set; } = null!;

        [Required(ErrorMessage = "Course Status Is Required")]
        public string CourseStatus { get; set; } = null!;

        [Required(ErrorMessage = "Price Is Required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price Must Be Positive Value")]
        public decimal Price { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = " Capacity Is Required ")]
        [Range(0, 30, ErrorMessage = " The Maximun Students In Course Is 30 ")]
        public int Capacity { get; set; }

        public int categoryId { get; set; }

        public string? categoryDescription { get; set; }
        public List<string> ImageUrls { get; set; } = [];


    }
}
