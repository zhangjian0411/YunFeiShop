using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;
using ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate;

namespace ZhangJian.YunFeiShop.Services.Carts.Infrastructure
{
    public class CartRepository : ICartRepository
    {
        private readonly CartContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public CartRepository(CartContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Cart cart)
        {
            _context.Add(cart);
        }

        public void Update(Cart cart)
        {
            _context.Update(cart);
        }

        public async Task<Cart> GetAsync(Guid buyerId)
        {
            return await _context.Carts.Include(c => c.Lines).SingleOrDefaultAsync(o => o.BuyerId == buyerId);
        }
    }
}