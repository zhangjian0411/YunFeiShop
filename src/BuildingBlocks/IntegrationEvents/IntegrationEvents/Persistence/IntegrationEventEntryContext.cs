using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence
{
    public class IntegrationEventEntryContext : DbContext
    {
        public IntegrationEventEntryContext(DbContextOptions<IntegrationEventEntryContext> options) : base(options)
        {
        }

        public DbSet<IntegrationEventEntry> IntegrationEventEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IntegrationEventEntry>(ConfigureIntegrationEventEntry);
        }

        void ConfigureIntegrationEventEntry(EntityTypeBuilder<IntegrationEventEntry> builder)
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