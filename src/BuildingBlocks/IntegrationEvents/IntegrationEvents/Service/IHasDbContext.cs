using Microsoft.EntityFrameworkCore;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Services
{
    public interface IHasDbContext
    {
        DbContext DbContext { get; }
    }
}