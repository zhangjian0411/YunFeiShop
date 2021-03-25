using System;
using System.Threading.Tasks;

namespace ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
