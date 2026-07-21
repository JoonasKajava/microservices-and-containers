using Microsoft.Extensions.Options;
using Reservation.Contracts;

namespace Reservation.Repositories;

public interface IInventoryRepository
{
    public Task<GetEquipmentResponse?> GetEquipmentByIdAsync(Guid id);
}

public class InventoryRepository(
    IHttpClientFactory clientFactory,
    IOptions<ReservationOptions> options) : IInventoryRepository
{
    public async Task<GetEquipmentResponse?> GetEquipmentByIdAsync(Guid id)
    {
        using var client = clientFactory.CreateClient();

        return await client.GetFromJsonAsync<GetEquipmentResponse>($"{options.Value.InventoryServiceUrl}/api/v1/equipment/{id}");
    }
}