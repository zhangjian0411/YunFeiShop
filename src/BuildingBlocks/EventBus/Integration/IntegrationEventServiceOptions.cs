using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ZhangJian.YunFeiShop.BuildingBlocks.Integration
{
    public class IntegrationEventServiceOptions
    {
        public string ClientName { get; set; }
        public Action<DbContextOptionsBuilder, DbConnection> EventLogContextOptionsAction { get; set; }
    }
}