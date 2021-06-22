using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ZhangJian.YunFeiShop.BFF.SPA.Shopping.Services;

namespace ZhangJian.YunFeiShop.BFF.SPA.Shopping.Common
{
    public class ApplicationController : ControllerBase
    {
        public new ApplicationUser User => new ApplicationUser(base.User);
    }

    public class ApplicationUser
    {
        public ClaimsPrincipal Principal { get; }
        public string Name => Principal?.Identity?.Name;
        public string Id => Principal?.FindFirst("sub")?.Value;

        public ApplicationUser(ClaimsPrincipal principal)
        {
            Principal = principal;
        }
    }
}