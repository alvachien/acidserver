using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AcidServer.Tests;

/// <summary>
/// Creates a UserManager<ApplicationUser> backed by in-memory SQLite for testing.
/// </summary>
public static class UserManagerFixture
{
    public static UserManager<ApplicationUser> Create(ApplicationDbContextFixture dbFixture)
    {
        var services = new ServiceCollection();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(dbFixture.Connection));

        services.AddLogging();
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        var serviceProvider = services.BuildServiceProvider();

        // Ensure tables exist before UserManager tries to access them.
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        return serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    }
}
