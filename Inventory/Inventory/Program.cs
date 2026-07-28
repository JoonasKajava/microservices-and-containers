using Inventory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<InventoryOptions>()
    .Bind(builder.Configuration.GetSection(InventoryOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHostedService<InventoryPublisher>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}