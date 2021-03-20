using Microsoft.EntityFrameworkCore;

namespace ZhangJian.YunFeiShop.BuildingBlocks.Integration.DefaultService
{
    interface IHasDataContext
    {
        DbContext DataContext { get; }
    }
}