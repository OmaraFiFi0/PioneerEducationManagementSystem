using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Shared.DTOs.CategoryDTOs
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
