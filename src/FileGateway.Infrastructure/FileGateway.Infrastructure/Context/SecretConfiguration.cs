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
            builder.HasIndex(p => p.FileName)
                .IsUnique();
            builder.Property(p => p.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
