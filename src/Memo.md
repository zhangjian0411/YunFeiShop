# Memo

## If integration event proccess failed after it received from rabbitmq server, it should handled with a Dead Letter Exchange (DLX)

```txt
See line 226 
at private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs) 
at BuildingBlocks\IntegrationEvents\EventBus.RabbitMQ\EventBusRabbitMQ.cs
```

## When Ordering service in not online, the `UserCheckoutAcceptedIntegrationEvent` is not proccessed and `OrderStartedIntegrationEvent` is not produced. So the cart will not remove cartlines. It will produce duplicate orders

```txt
Does it need to handle? Or this is acceptable?
```

## AutoMapper: Destination object type must have a constructor with 0 args and properties must have set accessor, so the mapping can work

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

## EF Core: EF Core does not use cache by default

## EF Core: DbContext.Update() method can also add new entity

## Docker: docker 18.03 加入了一个 feature，在容器中可以通过 host.docker.internal来访问主机
