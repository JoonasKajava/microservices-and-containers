using System.Net;
using System.Text;
using Inventory.Contracts;
using Inventory.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;

namespace Inventory.Tests;

public class TestEquipmentController(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    public static Guid TestEquipmentId = Guid.NewGuid();

    private readonly HttpClient _client = factory.CreateClient(new WebApplicationFactoryClientOptions()
    {
        AllowAutoRedirect = false
    });

    [Fact]
    public async Task Test__CreateEquipment__FailsOnEmptyBody()
    {
        var response = await _client.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("http://localhost/api/v1/Equipment"),
            Content = new StringContent("{}", Encoding.UTF8, "application/json")
        });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Test__CreateEquipment__IsSuccessWhenBodyHasName()
    {
        var response = await _client.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri("http://localhost/api/v1/Equipment"),
            Content = JsonContent.Create(new CreateEquipment()
            {
                Name = "Test equipment"
            })
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Test__GetEquipmentByIdAsync__Returns200()
    {
        var response = await _client.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost/api/v1/Equipment/{TestEquipmentId}"),
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Test__GetEquipmentByIdAsync__Returns404()
    {
        var response = await _client.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"http://localhost/api/v1/Equipment/{Guid.NewGuid()}"),
        });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var mocker = new AutoMocker();

        mocker.GetMock<IInventoryRepository>().Setup(repository =>
                repository.GetEquipmentByIdAsync(TestEquipmentController.TestEquipmentId))
            .ReturnsAsync(new Entities.Equipment
            {
                Creator = "Test",
                Name = "Laptop"
            });

        builder.ConfigureServices(services =>
        {
            // services.Configure<InventoryOptions>(options => { options.DatabaseUrl = "dummy"; });
            services.AddSingleton(mocker.GetMock<IInventoryRepository>().Object);
        });
        builder.UseEnvironment("Testing");
    }
}