using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Reservation;

public class ReservationStateService(
    ILogger<ReservationStateService> logger,
    IOptions<ReservationOptions> options
) : IHostedService, IDisposable, IAsyncDisposable
{
    private Timer? _timer;

    private PublisherSocket? _socket;

    private readonly TimeSpan _processInterval = TimeSpan.FromMinutes(1);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("ReservationStateService is starting.");

        _socket = new PublisherSocket();
        logger.LogInformation("Binding publisher socket to addr: {addr}", options.Value.PublisherSocketBind);
        _socket.Bind(options.Value.PublisherSocketBind);

        _timer = new Timer(Process, null, TimeSpan.Zero, _processInterval);


        return Task.CompletedTask;
    }

    private void Process(object? state)
    {
        ArgumentNullException.ThrowIfNull(_socket);

        logger.LogInformation("Processing reservations.");

        _socket.SendMoreFrame("reservation").SendFrame("update");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("ReservationStateService is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        _socket?.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_timer != null) await _timer.DisposeAsync();
    }
}