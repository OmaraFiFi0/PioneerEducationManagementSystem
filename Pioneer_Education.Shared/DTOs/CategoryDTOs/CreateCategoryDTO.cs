using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Shared.DTOs.CategoryDTOs
{
    public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Category Name Is Required")]
        [MaxLength(150, ErrorMessage = "Max Length To Category Name Is 150 Characters")]
        [MinLength(3, ErrorMessage = "Min Length To Category Name Is 3 Characters")]
        public string Category_Name { get; set; } = null!;

        [MaxLength(200, ErrorMessage = "Max Length To Category Description Is 200 Characters")]
        [MinLength(3, ErrorMessage = "Min Length To Category Description Is 3 Characters")]
        public string? Category_Description { get; set; } = null!;
    }
}
