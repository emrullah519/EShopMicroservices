using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f=>f.Id)
                .HasConversion(write => write.Value, read => CustomerId.Of(read));
            builder.Property(f => f.Name).HasMaxLength(100).IsRequired();
            builder.Property(f => f.Email).HasMaxLength(255);
            builder.HasIndex(f=>f.Email).IsUnique();
        }
    }
}
