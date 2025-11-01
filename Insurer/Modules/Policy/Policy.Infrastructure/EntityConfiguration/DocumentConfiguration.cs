namespace Policy.Infrastructure.EntityConfiguration;

internal class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(pk => pk.Id);

        builder.Property(d => d.FileName)
          .HasMaxLength(255)
          .IsRequired();


        builder.Property(d => d.FileType)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(d => d.UploadedDate).HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(p => p.Policy)
            .WithMany(d => d.Documents)
            .HasForeignKey(fk => fk.PolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pc => pc.PolicyClaim)
           .WithMany(d => d.Documents)
           .HasForeignKey(fk => fk.PolicyClaimId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(i => i.PolicyId);
        builder.HasIndex(i => i.PolicyClaimId);
    }
}
