using Inventory;
using Inventory.Context;
using Inventory.Extensions;
using Inventory.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<IInventoryPublisher, InventoryPublisher>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

builder.Services.AddDbContext<InventoryDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("InventoryDb"));
});

builder.AddJwtAuthentication();

builder.Services.AddOptions<InventoryOptions>()
    .Bind(builder.Configuration.GetSection(InventoryOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    app.MigrateDatabase();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}