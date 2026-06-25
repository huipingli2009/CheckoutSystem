using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheckoutSystem.Infrastructure.Persistence.Configurations
{
    public class CheckoutItemConfiguration : IEntityTypeConfiguration<CheckoutSystem.Domain.Entities.CheckoutItem>
    {
        public void Configure(EntityTypeBuilder<CheckoutSystem.Domain.Entities.CheckoutItem> builder)
        {
            builder.ToTable("CheckoutItems");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id)
                .ValueGeneratedNever();
            builder.Property(c => c.ProductId)
                .IsRequired();
            builder.Property(c => c.ProductName)
               .HasMaxLength(200)
               .IsRequired();
            builder.Property(c => c.Quantity)
                .IsRequired();            

            // Flattening the Money Value Object structure seamlessly into the item columns
            builder.OwnsOne(c => c.Price, price =>
            {
                price.Property(p => p.Amount)
                    .HasColumnName("PriceAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                price.Property(p => p.Currency)
                    .HasColumnName("PriceCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });
        }
    }
}
 
