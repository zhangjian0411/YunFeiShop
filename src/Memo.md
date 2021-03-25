## If integration event proccess failed after it received from rabbitmq server, it should handled with a Dead Letter Exchange (DLX).
    See line 226 
    at private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs) 
    at BuildingBlocks\IntegrationEvents\EventBus.RabbitMQ\EventBusRabbitMQ.cs 
