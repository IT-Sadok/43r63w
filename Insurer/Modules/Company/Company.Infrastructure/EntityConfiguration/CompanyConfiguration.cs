using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.EntityConfiguration;

internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Domain.Entity.Company>
{
    public void Configure(EntityTypeBuilder<Domain.Entity.Company> builder)
    {
        builder.Property(e => e.CompanyName)
            .IsRequired();

        builder.Property(e => e.RegistrationNumber)
            .IsRequired();
        
        builder.HasMany(e => e.Documents)
            .WithOne(e => e.Company)
            .OnDelete(DeleteBehavior.Cascade);
    }
}