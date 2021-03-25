using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure;

namespace ZhangJian.YunFeiShop.Services.Ordering.Infrastructure
{
    public class OrderingContext : DbContextBase
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        public OrderingContext(DbContextOptions<OrderingContext> options, IMediator mediator) : base(options, mediator) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderLineEntityTypeConfiguration());
        }
    }

    class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> orderConfiguration)
        {
            orderConfiguration.ToTable("orders");

            orderConfiguration.HasKey(o => o.Id);

            orderConfiguration.Ignore(b => b.DomainEvents);


            // orderConfiguration
            //     .Property<Guid>("_buyerId")
            //     .UsePropertyAccessMode(PropertyAccessMode.Field)
            //     .HasColumnName("BuyerId")
            //     .IsRequired(true);


            var navigation = orderConfiguration.Metadata.FindNavigation(nameof(Order.OrderLines));

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

        }
    }

    class OrderLineEntityTypeConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> orderLineConfiguration)
        {
            orderLineConfiguration.ToTable("orderLines");

            orderLineConfiguration.HasKey(o => o.Id);

            orderLineConfiguration.Ignore(b => b.DomainEvents);
        }
    }
}
