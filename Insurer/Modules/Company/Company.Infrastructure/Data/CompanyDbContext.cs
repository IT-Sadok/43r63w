using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Data;

public class CompanyDbContext(DbContextOptions<CompanyDbContext> options) : DbContext(options)
{
    public DbSet<Domain.Entity.Company> Companies { get; set; }
    
    public DbSet<Domain.Entity.Document> Documents { get; set; }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("company");
    }
}