using MassTransit;
using TestMobiBuy.Application.Interfaces;

namespace TestMobiBuy.Application.Services;

public class MessageBusService : IMessageBusService
{
    private readonly IBus _bus;

    public MessageBusService(IBus bus)
    {
        _bus = bus;
    }

    public async Task Publish<T>(T message) where T : class
    {
        await _bus.Publish(message);
    }
} 