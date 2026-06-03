using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Core.Entities.CouresModule
{
    public class CourseImage : BaseEntity<int>
    {
        public string PictureUrl { get; set; } = null!;

        public int CourseId { get; set; }
    }
}
