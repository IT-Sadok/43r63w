using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain;
using User.Domain.Entity;

namespace User.Infrastructure.EntityConfiguration;

internal sealed class AgentConfiguration : IEntityTypeConfiguration<Agent>
{
    public void Configure(EntityTypeBuilder<Agent> builder)
    {
        builder.OwnsOne(e => e.Address, a =>
        {
            a.Property(e => e.Country).HasColumnName("Country");
            a.Property(e => e.City).HasColumnName("City");
            a.Property(e => e.Street).HasColumnName("Street");
            a.Property(e => e.ZipCode).HasColumnName("ZipCode");
        });
    }
}