using System.Text.Json.Serialization;
using Inventory.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Extensions;

public static class WebApplicationExtensions
{
    public static void MigrateDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        dbContext.Database.Migrate();
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetSection(InventoryOptions.SectionName).Get<InventoryOptions>() ??
                            throw new InvalidOperationException("InventoryOptions missing");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MetadataAddress = $"{configuration.JwtAuthority}/.well-known/openid-configuration";

                options.BackchannelHttpHandler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback
                        = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidAudience = configuration.JwtAudience,
                    ValidIssuer = configuration.JwtAuthority,
                    ValidateIssuerSigningKey = true
                };
            });
    }
}