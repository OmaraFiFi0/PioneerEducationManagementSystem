using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pioneer_Education.Core.Entities.CouresModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Infrastructure.Data.Configurations
{
    public class CourseConfigurations : BaseConfigurations<Course, int>, IEntityTypeConfiguration<Course>
    {
        public new void Configure(EntityTypeBuilder<Course> builder)
        {
            base.Configure(builder);


            builder.Property(X => X.Id).UseIdentityColumn(100, 1);
            builder.Property(X => X.Price).HasPrecision(8, 2);
            builder.Property(X => X.CourseName).HasMaxLength(100);
            builder.Property(X => X.Description).HasMaxLength(500);

        }
    }
}
