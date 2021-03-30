using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Infrastructure
{
    public class CartContext : DbContextBase
    {
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartLine> CartLines { get; set; }

        public CartContext(DbContextOptions<CartContext> options, IMediator mediator, ILogger<CartContext> logger) : base(options, mediator, logger) { }
    }

}