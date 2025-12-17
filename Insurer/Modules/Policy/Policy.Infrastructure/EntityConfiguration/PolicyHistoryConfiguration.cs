namespace Policy.Infrastructure.EntityConfiguration;

internal class PolicyHistoryConfiguration : IEntityTypeConfiguration<Policy.Domain.Entities.PolicyHistory>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.PolicyHistory> builder)
    {
        builder.HasKey(pk => pk.Id);
        
        builder
            .HasOne(h => h.Policy)
            .WithMany(p => p.PolicyHistories)
            .HasForeignKey(h => h.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
