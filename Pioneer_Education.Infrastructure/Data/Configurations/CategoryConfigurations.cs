using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pioneer_Education.Core.Entities.CategoryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Infrastructure.Data.Configurations
{
    public class CategoryConfigurations : BaseConfigurations<Category, int>, IEntityTypeConfiguration<Category>
    {
        public new void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);

            builder.Property(I => I.Id).UseIdentityColumn(10, 10);
            builder.Property(X => X.Description).HasMaxLength(200);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.HasIndex(X => X.Name).IsUnique();


            builder.HasMany(X => X.Courses)
                .WithOne(X => X.Category)
                .HasForeignKey(X => X.CategoryId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
