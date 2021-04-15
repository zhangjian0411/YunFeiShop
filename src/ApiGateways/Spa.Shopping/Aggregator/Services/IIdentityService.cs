using System;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services
{
    public interface IIdentityService
    {
        Guid GetUserIdentity();
        string GetUserName();
    }
}