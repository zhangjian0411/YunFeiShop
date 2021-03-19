using RabbitMQ.Client;
using System;

namespace ZhangJian.YunFeiShop.BuildingBlocks.EventBus.RabbitMQ
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
