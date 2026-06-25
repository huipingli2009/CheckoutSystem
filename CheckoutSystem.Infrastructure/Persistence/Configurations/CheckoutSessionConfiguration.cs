using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CheckoutSystem.Infrastructure.Persistence.Configurations
{
    public class CheckoutSessionConfiguration : IEntityTypeConfiguration<CheckoutSystem.Domain.Entities.CheckoutSession>
    {
        public void Configure(EntityTypeBuilder<CheckoutSystem.Domain.Entities.CheckoutSession> builder)
        {
            builder.ToTable("CheckoutSessions");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.CustomerId)
                .IsRequired();

            builder.Property(c => c.Status)
                .HasConversion<string>()  // stores enum values as readable strings
                .HasMaxLength(20)   
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            // Configure the strong relationship to CheckoutItems (1-to-many aggregate boundary)
            builder.HasMany(c => c.Items)
                .WithOne()
                .HasForeignKey("CheckoutSessionId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(c => c.Items)
                .Metadata
                .SetField("_items");                 
        }
    }
}
