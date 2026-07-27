using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reservation.Context;

namespace Reservation.Extensions;

public static class WebApplicationExtensions
{
    public static void MigrateDatabase(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
        dbContext.Database.Migrate();
    }

    public static void AddJwtAuthentication(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration.GetSection(ReservationOptions.SectionName).Get<ReservationOptions>() ??
                            throw new InvalidOperationException("ReservationOptions missing");
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MetadataAddress = $"{configuration.JwtAuthority}/.well-known/openid-configuration";

                options.MapInboundClaims = false;

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