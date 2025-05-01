using MassTransit;

namespace TestMobiBuy.Application.Interfaces;

public interface IMessageBusService
{
    Task Publish<T>(T message) where T : class;
} 