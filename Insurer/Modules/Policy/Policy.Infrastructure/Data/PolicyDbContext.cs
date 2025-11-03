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

    public DbSet<UserPayment> UserPayments { get; set; }

    public DbSet<CompanyPayment> CompanyPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("policy");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PolicyDbContext).Assembly);

        modelBuilder.HasSequence<int>("PolicyNumbers", schema: "policy")
            .StartsAt(1000)
            .IncrementsBy(1);
    }
}
