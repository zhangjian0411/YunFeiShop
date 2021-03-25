namespace ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Domain
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}