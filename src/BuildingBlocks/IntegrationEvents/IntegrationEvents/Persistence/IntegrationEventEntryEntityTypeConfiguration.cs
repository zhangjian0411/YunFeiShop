using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence
{
    public class IntegrationEventEntryEntityTypeConfiguration
            : IEntityTypeConfiguration<IntegrationEventEntry>
    {
        public void Configure(EntityTypeBuilder<IntegrationEventEntry> builder)
        {
            builder.ToTable("IntegrationEventEntries");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();
        }
    }
}