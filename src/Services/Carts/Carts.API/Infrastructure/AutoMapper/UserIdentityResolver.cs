using System;
using AutoMapper;
using ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.Services;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.AutoMapper
{
    public class UserIdentityResolver : IValueResolver<object, object, Guid>
    {
        private readonly IIdentityService _identityService;
        public UserIdentityResolver(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Guid Resolve(object source, object destination, Guid destMember, ResolutionContext context)
        {
            return _identityService.GetUserIdentity();
        }
    }
}