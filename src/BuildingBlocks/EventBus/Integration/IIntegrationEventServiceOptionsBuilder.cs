using System;
using System.Collections.Generic;

namespace ZhangJian.YunFeiShop.BuildingBlocks.Integration
{
    public interface IIntegrationEventServiceOptionsBuilder
    {
        IIntegrationEventServiceOptionsBuilder AddIntegrationEventLog();
    }
}