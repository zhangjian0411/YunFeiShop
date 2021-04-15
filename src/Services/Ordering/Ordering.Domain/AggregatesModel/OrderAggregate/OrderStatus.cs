namespace ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public enum OrderStatus
    {
        Submitted,
        AwaitingValidation,
        StockConfirmed,
        Paid,
        Shipped,
        Completed,
        Cancelled
    }
}