using Microsoft.EntityFrameworkCore;
using User.Domain;
using User.Domain.Entity;

namespace User.Infrastructure.Data;

public class UserDbContext(DbContextOptions<UserDbContext> options)
    : DbContext(options)
{
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Agent> Agents { get; set; }
    
    public DbSet<Invintation> Invitations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);

        modelBuilder.HasDefaultSchema("user");
    }
}