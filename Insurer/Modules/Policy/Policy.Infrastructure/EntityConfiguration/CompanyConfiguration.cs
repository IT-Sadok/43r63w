

namespace Policy.Infrastructure.EntityConfiguration;

internal class CompanyConfiguration : IEntityTypeConfiguration<CompanyPayment>
{
    public void Configure(EntityTypeBuilder<CompanyPayment> builder)
    {
       builder.HasOne(p => p.Claim)
              .WithMany(p => p.CompanyPayments)
              .HasForeignKey(fk => fk.ClaimId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}
