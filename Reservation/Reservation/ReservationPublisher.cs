using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using Reservation.Contracts;

namespace Reservation;

public interface IReservationPublisher
{
    public Task ReservationCreated(Entities.Reservation reservation);
    public Task ReservationCanceled(Guid reservationId, Guid equipmentId);
    public Task ReservationStarted(Entities.Reservation reservation);
    public Task ReservationReturned(Entities.Reservation reservation);
}

public class ReservationPublisher : IReservationPublisher, IDisposable
{
    private readonly ILogger<ReservationPublisher> _logger;
    private readonly PublisherSocket? _socket;

    public ReservationPublisher(ILogger<ReservationPublisher> logger, IOptions<ReservationOptions> options)
    {
        _logger = logger;

        _logger.LogInformation("Binding {name} to {addr}", nameof(ReservationPublisher),
            options.Value.PublisherBindAddr);
        _socket = new PublisherSocket();
        _socket.Bind(options.Value.PublisherBindAddr);
    }

    private Task Send<T>(string topic, T reservation)
    {
        ArgumentNullException.ThrowIfNull(_socket);

        var message = JsonConvert.SerializeObject(reservation);

        _logger.LogInformation("Sending message with topic: {topic}, contents: {}", topic, message);
        _socket.SendMoreFrame(topic).SendFrame(message);
        return Task.CompletedTask;
    }

    public Task ReservationCreated(Entities.Reservation reservation)
    {
        return Send("reservation.created", new ReservationStatusChangeEvent()
        {
            ReservationId = reservation.ReservationId,
            EquipmentId = reservation.EquipmentId
        });
    }

    public Task ReservationCanceled(Guid reservationId, Guid equipmentId)
    {
        return Send("reservation.canceled", new ReservationStatusChangeEvent()
        {
            ReservationId = reservationId,
            EquipmentId = equipmentId
        });
    }

    public Task ReservationStarted(Entities.Reservation reservation)
    {
        return Send("reservation.started", new ReservationStatusChangeEvent()
        {
            ReservationId = reservation.ReservationId,
            EquipmentId = reservation.EquipmentId
        });
    }

    public Task ReservationReturned(Entities.Reservation reservation)
    {
        return Send("reservation.returned", new ReservationStatusChangeEvent()
        {
            ReservationId = reservation.ReservationId,
            EquipmentId = reservation.EquipmentId
        });
    }

    public void Dispose()
    {
        _socket?.Dispose();
    }
}