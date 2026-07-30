using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using Reservation.Repositories;

namespace Reservation;

public class InventorySubscriber(
    ILogger<InventorySubscriber> logger,
    IOptions<ReservationOptions> options,
    IServiceProvider serviceProvider
) : BackgroundService
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
            using var scope = serviceProvider.CreateScope();
            var reservationsRepository = scope.ServiceProvider.GetRequiredService<IReservationsRepository>();

            var topic = socket.ReceiveFrameString();
            var msg = socket.ReceiveFrameString();

            logger.LogInformation("Equipment with id: {id} deleted", msg);

            await reservationsRepository.DeleteReservationsForEquipmentAsync(Guid.Parse(msg));
        }
    }


}