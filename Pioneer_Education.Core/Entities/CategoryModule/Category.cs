using Pioneer_Education.Core.Entities.CouresModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Core.Entities.CategoryModule
{
    public class Category : BaseEntity<int>
    {

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<Course>? Courses { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
