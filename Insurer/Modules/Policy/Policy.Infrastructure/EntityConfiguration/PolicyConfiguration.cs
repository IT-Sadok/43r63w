

namespace Policy.Infrastructure.EntityConfiguration;

internal class PolicyConfiguration : IEntityTypeConfiguration<Domain.Entities.Policy>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Policy> builder)
    {

        builder.HasKey(pk => pk.Id);

        builder.Property(p => p.PolicyNumber)
               .IsRequired()
               .HasMaxLength(50);


        builder.Property(p => p.PolicyNumber)
            .IsRequired()
            .HasMaxLength(50);


        builder.HasMany(p => p.Documents)
            .WithOne(d => d.Policy)
            .HasForeignKey(d => d.PolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.PolicyClaims)
            .WithOne(pc => pc.Policy)
            .HasForeignKey(fk => fk.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasIndex(i => i.PolicyNumber)
            .IsUnique();
    }
}
