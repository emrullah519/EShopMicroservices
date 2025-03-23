using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasConversion(w => w.Value, r => OrderId.Of(r));

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(x => x.CustomerId)
                .IsRequired();

            builder.HasMany<OrderItem>()
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

            builder.ComplexProperty(p => p.OrderName, builder =>
            {
                builder.Property(f => f.Value)
                .HasColumnName(nameof(Order.OrderName))
                .HasMaxLength(100)
                .IsRequired();
            });

            builder.ComplexProperty(p => p.ShippingAddress, builder =>
            {
                builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
                builder.Property(p => p.LastName).HasMaxLength(50).IsRequired();
                builder.Property(p => p.EmailAddress).HasMaxLength(50);
                builder.Property(p => p.AddressLine).HasMaxLength(180).IsRequired();
                builder.Property(p => p.Country).HasMaxLength(50);
                builder.Property(p => p.State).HasMaxLength(50);
                builder.Property(p => p.ZipCode).HasMaxLength(5).IsRequired();
            });

            builder.ComplexProperty(p => p.BillingAddress, builder =>
            {
                builder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
                builder.Property(p => p.LastName).HasMaxLength(50).IsRequired();
                builder.Property(p => p.EmailAddress).HasMaxLength(50);
                builder.Property(p => p.AddressLine).HasMaxLength(180).IsRequired();
                builder.Property(p => p.Country).HasMaxLength(50);
                builder.Property(p => p.State).HasMaxLength(50);
                builder.Property(p => p.ZipCode).HasMaxLength(5).IsRequired();
            });

            builder.ComplexProperty(p => p.Payment, builder =>
            {
                builder.Property(p => p.CardName).HasMaxLength(50);
                builder.Property(p => p.CardNumber).HasMaxLength(24).IsRequired();
                builder.Property(p => p.Expiration).HasMaxLength(10);
                builder.Property(p => p.CVV).HasMaxLength(3);
                builder.Property(p => p.PaymentMethod);
            });

            builder.Property(p => p.Status)
                .HasDefaultValue(OrderStatus.Draft)
                .HasConversion(w => w.ToString(), r => (OrderStatus)Enum.Parse(typeof(OrderStatus), r));

            builder.Property(p => p.TotalPrice);
        }
    }
}
