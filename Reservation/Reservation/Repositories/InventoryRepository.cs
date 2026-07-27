using Microsoft.Extensions.Options;
using Reservation.Contracts;

namespace Reservation.Repositories;

public interface IInventoryRepository
{
    public Task<GetEquipmentResponse?> GetEquipmentByIdAsync(Guid id, string authorization);
}

public class InventoryRepository(
    IHttpClientFactory clientFactory,
    IOptions<ReservationOptions> options) : IInventoryRepository
{
    public async Task<GetEquipmentResponse?> GetEquipmentByIdAsync(Guid id, string authorization)
    {
        using var client = clientFactory.CreateClient();
        client.DefaultRequestHeaders.Add("Authorization", authorization);

        return await client.GetFromJsonAsync<GetEquipmentResponse>(
            $"{options.Value.InventoryServiceUrl}/api/v1/equipment/{id}");
    }
}