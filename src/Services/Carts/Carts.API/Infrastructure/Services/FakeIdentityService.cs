using System;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.Services
{
    public class FakeIdentityService : IIdentityService
    {
        public FakeIdentityService() { }

        public Guid GetUserIdentity()
        {
            return new Guid("78999999-9999-9999-9999-999999990000");
        }

        public string GetUserName()
        {
            return "Fake User Name";
        }
    }
}
