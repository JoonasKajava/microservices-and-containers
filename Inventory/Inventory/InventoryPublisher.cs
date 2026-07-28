using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Inventory;

public class InventoryPublisher : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly PublisherSocket? _socket;
    private readonly ILogger<InventoryPublisher> _logger;

    public InventoryPublisher(ILogger<InventoryPublisher> logger, IOptions<InventoryOptions> options)
    {
        _logger = logger;

        _logger.LogInformation("Binding InventoryPublisher to {addr}", options.Value.InventoryPublisherBindAddr);
        _socket = new PublisherSocket();
        _socket.Bind(options.Value.InventoryPublisherBindAddr);
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DeleteEquipment, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DeleteEquipment(object? state)
    {
        ArgumentNullException.ThrowIfNull(_socket);
        var id = Guid.NewGuid().ToString();
        _socket.SendMoreFrame("delete").SendFrame(id);
        _logger.LogInformation("Equipment with id: {id} deleted", id);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("InventoryPublisher is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}