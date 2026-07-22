using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Inventory;

public class ReservationStateSubscriber(ILogger<ReservationStateSubscriber> logger, IOptions<InventoryOptions> options)
    : IHostedService, IDisposable
{
    private Timer? _timer;

    private SubscriberSocket? _socket;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("ReservationStateSubscriber is starting.");

        _socket = new SubscriberSocket();
        logger.LogInformation("Binding subscriber socket to addr: {addr}", options.Value.ReservationStatePubAddr);
        _socket.Connect(options.Value.ReservationStatePubAddr);
        _socket.Subscribe(options.Value.ReservationStatePubTopic);


        while (true)
        {
            var topic = _socket.ReceiveFrameString();
            var msg = _socket.ReceiveFrameString();

            logger.LogInformation("Received message on topic {topic}: {msg}", topic, msg);
        }


        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("ReservationStateSubscriber is stopping.");

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _socket?.Dispose();
    }
}