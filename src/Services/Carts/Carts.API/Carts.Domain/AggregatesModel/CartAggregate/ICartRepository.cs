using System;
using System.Threading.Tasks;
using ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain;

namespace ZhangJian.YunFeiShop.Services.Carts.Domain.AggregatesModel.CartAggregate
{
    public interface ICartRepository: IRepository<Cart>
    {
        void Update(Cart item);
        Task<Cart> GetAsync(Guid buyerId);
    }
}