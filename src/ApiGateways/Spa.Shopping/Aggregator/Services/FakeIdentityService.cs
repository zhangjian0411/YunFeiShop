using System;

namespace ZhangJian.YunFeiShop.BFF.Spa.Shopping.HttpAggregator.Services
{
    public class FakeIdentityService : IIdentityService
    {
        public Guid GetUserIdentity()
        {
            return new Guid("88888888-9999-9999-9999-999999990000");
        }

        public string GetUserName()
        {
            return "Fake User Name";
        }
    }
}