using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Reservation;

public class InventorySubscriber(
    ILogger<InventorySubscriber> logger,
    IOptions<ReservationOptions> options) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Inventory subscriber is starting.");

        using var socket = new SubscriberSocket();
        logger.LogInformation("Connecting inventory subscriber to {addr}", options.Value.InventorySubAddr);
        socket.Connect(options.Value.InventorySubAddr);
        socket.Subscribe("delete");

        while (!stoppingToken.IsCancellationRequested)
        {
            var topic = socket.ReceiveFrameString();
            var msg = socket.ReceiveFrameString();

            logger.LogInformation("Received delete message for id: {id}", msg);
        }
    }
}