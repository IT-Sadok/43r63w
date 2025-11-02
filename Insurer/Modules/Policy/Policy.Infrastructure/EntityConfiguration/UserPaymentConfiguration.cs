namespace Policy.Infrastructure.EntityConfiguration;

internal class UserPaymentConfiguration : IEntityTypeConfiguration<UserPayment>
{
    public void Configure(EntityTypeBuilder<UserPayment> builder)
    {
        builder.HasOne(p => p.Policy)
               .WithMany(p => p.UserPayments)
               .HasForeignKey(fk => fk.PolicyId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
