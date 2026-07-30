using Microsoft.Extensions.Options;
using NetMQ;
using NetMQ.Sockets;

namespace Inventory;

public interface IInventoryPublisher
{
    public Task EquipmentDeleted(Guid id);
}

public class InventoryPublisher : IInventoryPublisher, IDisposable
{
    private readonly ILogger<InventoryPublisher> _logger;
    private readonly PublisherSocket? _socket;

    public InventoryPublisher(ILogger<InventoryPublisher> logger, IOptions<InventoryOptions> options)
    {
        _logger = logger;

        _logger.LogInformation("Binding InventoryPublisher to {addr}", options.Value.InventoryPublisherBindAddr);
        _socket = new PublisherSocket();
        _socket.Bind(options.Value.InventoryPublisherBindAddr);
    }


    public void Dispose()
    {
        _socket?.Dispose();
    }

    public Task EquipmentDeleted(Guid id)
    {
        ArgumentNullException.ThrowIfNull(_socket);
        _socket.SendMoreFrame("delete").SendFrame(id.ToString());
        _logger.LogInformation("Equipment with id: {id} deleted", id);
        return Task.CompletedTask;
    }
}