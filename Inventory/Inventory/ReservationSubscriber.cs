using Inventory.Contracts;
using Inventory.Entities;
using Inventory.Repositories;
using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace Inventory;

public class ReservationSubscriber(
    ILogger<ReservationSubscriber> logger,
    IOptions<InventoryOptions> options,
    IServiceProvider serviceProvider
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Reservation subscriber is starting.");

        using var socket = new SubscriberSocket();
        logger.LogInformation("Connecting Reservation subscriber to {addr}", options.Value.ReservationSubAddr);
        socket.Connect(options.Value.ReservationSubAddr);
        socket.SubscribeToAnyTopic();

        while (!stoppingToken.IsCancellationRequested)
        {
            var topic = socket.ReceiveFrameString();
            var msg = socket.ReceiveFrameString();
            await HandleMessage(topic, msg);
        }
    }

    private async Task HandleMessage(string topic, string msg)
    {
        using var scope = serviceProvider.CreateScope();
        var inventoryRepository = scope.ServiceProvider.GetRequiredService<IInventoryRepository>();

        logger.LogInformation("Received message with topic: {topic} and content: {msg}", topic, msg);

        var payload = JsonConvert.DeserializeObject<ReservationStatusChangeEvent>(msg);

        if (payload is null)
        {
            logger.LogError("Failed to deserialize: {msg}", msg);
            return;
        }

        EquipmentAvailability? availability = topic switch
        {
            "reservation.started" => EquipmentAvailability.InUse,
            "reservation.returned" => EquipmentAvailability.Available,
            "reservation.canceled" => EquipmentAvailability.Available,
            _ => null
        };

        if (availability is null)
        {
            logger.LogWarning("Received message with unknown topic: {topic}", topic);
            return;
        }

        await inventoryRepository.UpdateEquipmentAsync(payload.EquipmentId,
            (equipment) => { equipment.Availability = availability.Value; });
    }
}