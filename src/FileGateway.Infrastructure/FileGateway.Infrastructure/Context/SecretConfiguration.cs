using FileGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileGateway.Infrastructure.Context
{
    public class SecretConfiguration : IEntityTypeConfiguration<Secret>
    {
        public void Configure(EntityTypeBuilder<Secret> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FileName)
                .HasConversion(
                v => string.IsNullOrWhiteSpace(v) ? null : v,
                v => v ?? string.Empty);

            builder.Property(p => p.ContentType)
                .HasConversion(
                v => string.IsNullOrWhiteSpace(v) ? null : v,
                v => v ?? string.Empty);

            builder.Property(p => p.FileName)
                .HasConversion(
                v => string.IsNullOrWhiteSpace(v) ? null : v,
                v => v ?? string.Empty);

            builder.Property(p => p.StoragePath)
                .HasConversion(
                v => string.IsNullOrWhiteSpace(v) ? null : v,
                v => v ?? string.Empty);

            builder.Property(p => p.BucketName)
                .HasConversion(
                v => string.IsNullOrWhiteSpace(v) ? null : v,
                v => v ?? string.Empty);


            builder.Property(p => p.FileName)
                .IsRequired(false);

            builder.Property(p => p.ContentType)
                .IsRequired(false);

            builder.Property(p => p.FileName)
                .IsRequired(false);

            builder.Property(p => p.StoragePath)
                .IsRequired(false);

            builder.Property(p => p.BucketName)
                .IsRequired(false);
        }
    }
}
