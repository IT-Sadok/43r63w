using Microsoft.EntityFrameworkCore;
using Policy.Domain.Entities;

namespace Policy.Infrastructure.Data;

public class PolicyDbContext : DbContext
{    
    public PolicyDbContext(DbContextOptions<PolicyDbContext> options) : base(options)
    {
    }

    public DbSet<PolicyClaim> PolicyClaims { get; set; }

    public DbSet<Policy.Domain.Entities.Policy> Policies { get; set; }

    public DbSet<PolicyHistory> PolicyHistory { get; set; }

    public DbSet<Document> Documents { get; set; }

    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("policy");
    }
}
