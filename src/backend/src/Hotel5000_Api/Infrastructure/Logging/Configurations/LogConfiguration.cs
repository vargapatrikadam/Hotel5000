using Core.Entities.Logging;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Logging
{
    public class LogConfiguration : IEntityTypeConfiguration<Log>, ILoggingConfigurationAggregate
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.ConfigureBaseEntityColumns();

            builder.EnableSoftDeletion();

            builder.Property(p => p.Timestamp)
                .IsRequired();

            builder.Property(p => p.Message)
                .IsRequired()
                .HasMaxLength(10000);

            builder.Property(p => p.Type)
                .IsRequired()
                .HasConversion<string>();
        }
    }
}