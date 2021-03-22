using Microsoft.EntityFrameworkCore;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Services
{
    public interface IHasDataContext
    {
        DbContext DataContext { get; }
    }
}