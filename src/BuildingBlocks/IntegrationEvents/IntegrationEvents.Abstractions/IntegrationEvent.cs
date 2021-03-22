using Newtonsoft.Json;
using System;

namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        [JsonConstructor]
        public IntegrationEvent(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }

        [JsonProperty]
        public Guid Id { get; }

        [JsonProperty]
        public DateTime CreationDate { get; }
    }
}
