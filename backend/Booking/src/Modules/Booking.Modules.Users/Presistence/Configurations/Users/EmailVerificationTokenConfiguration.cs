using Booking.Modules.Users.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Booking.Modules.Users.Presistence.Configurations.Users;

internal sealed class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
    public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.CreatedOnUtc)
            .IsRequired();
        builder.Property(e => e.ExpiresOnUtc)
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}