namespace Policy.Infrastructure.EntityConfiguration;

internal class PolicyClaimConfiguration : IEntityTypeConfiguration<PolicyClaim>
{
    public void Configure(EntityTypeBuilder<PolicyClaim> builder)
    {
        builder.HasKey(pk => pk.Id);


        builder.HasOne(pc => pc.Policy)
               .WithMany(p => p.PolicyClaims)
               .HasForeignKey(pc => pc.PolicyId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(pc => pc.ClaimNumber)
            .HasMaxLength(128);



        builder.HasMany(pc => pc.Documents)
               .WithOne(d => d.PolicyClaim)
               .HasForeignKey(d => d.PolicyClaimId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
