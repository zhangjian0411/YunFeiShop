## If integration event proccess failed after it received from rabbitmq server, it should handled with a Dead Letter Exchange (DLX).
    See line 226 
    at private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs) 
    at BuildingBlocks\IntegrationEvents\EventBus.RabbitMQ\EventBusRabbitMQ.cs 

## AutoMapper: Destination object type must have a constructor with 0 args and properties must have set accessor, so the mapping can work.
The constructor and set accessor could be `public`, `protected` or `private`.  
For example:
```c#
public class AddProductToCartCommand : IRequest<bool>
{
    public Guid BuyerId { get; init; }
    public Guid ProductId { get; init; }
}
```
or
```c#
public class AddProductToCartCommand : IRequest<bool>
{
    public Guid BuyerId { get; private set; }
    public Guid ProductId { get; private set; }

    private AddProductToCartCommand() { }

    public AddProductToCartCommand(Guid buyerId, Guid productId)
    {
        BuyerId = buyerId;
        ProductId = productId;
    }
}
```

## EF Core: EF Core does not use cache by default.

## EF Core: DbContext.Update() method can also add new entity.

## Commands: Command should be a record type in order to log its content. The record type overrides its ToString() method.