using System;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.Services
{
    public interface IIdentityService
    {
        Guid GetUserIdentity();

        string GetUserName();
    }
}
