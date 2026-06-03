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
    public class CourseImageConfigurations : BaseConfigurations<CourseImage, int>, IEntityTypeConfiguration<CourseImage>
    {
        public new void Configure(EntityTypeBuilder<CourseImage> builder)
        {
            base.Configure(builder);
            builder.Property(X => X.PictureUrl)
                      .HasMaxLength(500);
        }
    }
}
