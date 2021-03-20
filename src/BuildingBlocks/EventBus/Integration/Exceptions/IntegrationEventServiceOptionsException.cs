using System;

namespace ZhangJian.YunFeiShop.BuildingBlocks.Integration.Exceptions
{
    public class OptionRequiredException : Exception
    {
        public OptionRequiredException(string optionName) : base($"'{optionName}' is required when build a IntegrationEventServiceOptions.") { }
    }
}