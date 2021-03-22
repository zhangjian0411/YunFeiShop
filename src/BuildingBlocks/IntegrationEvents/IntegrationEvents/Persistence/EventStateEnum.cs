namespace ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence
{
    public enum EventStateEnum
    {
        NotPublished = 0,
        InProgress = 1,
        Published = 2,
        PublishedFailed = 3
    }
}