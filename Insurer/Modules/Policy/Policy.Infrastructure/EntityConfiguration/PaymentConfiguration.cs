


namespace Policy.Infrastructure.EntityConfiguration;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.HasOne(p => p.Policy)
               .WithMany(p => p.Payments)
               .HasForeignKey(fk => fk.PolicyId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.PolicyClaim)
          .WithMany(p => p.Payments)
          .HasForeignKey(fk => fk.PolicyClaimId)
          .OnDelete(DeleteBehavior.Restrict);
    }
}
