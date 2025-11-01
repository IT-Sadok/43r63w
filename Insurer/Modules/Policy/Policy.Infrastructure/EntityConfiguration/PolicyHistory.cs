using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Policy.Infrastructure.EntityConfiguration;

internal class PolicyHistory : IEntityTypeConfiguration<Policy.Domain.Entities.PolicyHistory>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.PolicyHistory> builder)
    {
        builder.HasKey(pk => pk.Id);


        builder.HasOne<Domain.Entities.Policy>()
            .WithMany(p => p.PolicyHistories)
            .HasForeignKey(fk => fk.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
