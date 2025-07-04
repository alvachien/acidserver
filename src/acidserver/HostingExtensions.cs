using Duende.IdentityServer;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityServerAspNetIdentity;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddCors();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<ApplicationUser>()
            .AddLicenseSummary();


        builder.Services.AddAuthentication();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors(option =>
        {
            option.WithOrigins(
#if DEBUG
#if USE_ALIYUN
                    "https://www.alvachien.com/hih", // HIH UI
                    "https://www.alvachien.com/gallery", // Gallery
                    "https://www.alvachien.com/math", // Math exercise
                    "https://www.alvachien.com/hihapi", // HIH API
                    "https://www.alvachien.com/galleryapi", // Gallery API
                    "https://www.alvachien.com/quizapi" // Quiz API
#else
                    "https://localhost:16001", // AC gallery
                    "https://localhost:29521", // AC HIH UI
                    "https://localhost:29528",  // AC HIH App
                    "https://localhost:44366", // AC HIH API
                    "https://localhost:25325", // AC Gallery API
                    // "https://localhost:20000", // Math exercise
                    // "https://localhost:54020"  // AC Quiz API
                    "https://localhost:44367"    // Knowledge builder
#endif
#endif
                    )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}
