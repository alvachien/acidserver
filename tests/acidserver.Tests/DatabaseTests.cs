using AcidServer.Tests;
using FluentAssertions;
using IdentityServerAspNetIdentity.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AcidServer.Tests;

public class DatabaseTests
{
    [Fact]
    public void CanCreateContext()
    {
        using var fixture = new ApplicationDbContextFixture();
        var context = fixture.CreateContext();
        context.Database.GetDbConnection().Should().NotBeNull();
        context.Database.GetDbConnection().State.Should().Be(System.Data.ConnectionState.Open);
    }

    [Fact]
    public void CanCreateAndQueryUsers()
    {
        using var fixture = new ApplicationDbContextFixture();
        using var context = fixture.CreateContext();
        context.Users.Should().BeEmpty();

        var user = new ApplicationUser { UserName = "testuser", Email = "test@example.com" };
        context.Users.Add(user);
        context.SaveChanges();

        var found = context.Users.First(u => u.UserName == "testuser");
        found.UserName.Should().Be("testuser");
    }

    [Fact]
    public void CanCreateMultipleIsolatedDatabases()
    {
        using var fixture1 = new ApplicationDbContextFixture();
        using var fixture2 = new ApplicationDbContextFixture();
        using var context1 = fixture1.CreateContext();
        using var context2 = fixture2.CreateContext();

        context1.Users.Add(new ApplicationUser { UserName = "user1" });
        context1.SaveChanges();

        context2.Users.Add(new ApplicationUser { UserName = "user2" });
        context2.SaveChanges();

        context1.Users.Should().HaveCount(1);
        context2.Users.Should().HaveCount(1);
    }
}

public class UserCreationTests
{
    [Fact]
    public async Task UserManager_CanCreateUser()
    {
        using var dbFixture = new ApplicationDbContextFixture();
        var userManager = UserManagerFixture.Create(dbFixture);

        var user = new ApplicationUser
        {
            UserName = "alice",
            Email = "alice@example.com",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Pass123$");

        result.Succeeded.Should().BeTrue();
        (await userManager.FindByNameAsync("alice")).Should().NotBeNull();
    }

    [Fact]
    public async Task UserManager_RejectsDuplicateUser()
    {
        using var dbFixture = new ApplicationDbContextFixture();
        var userManager = UserManagerFixture.Create(dbFixture);

        var user1 = new ApplicationUser { UserName = "bob", Email = "bob@example.com" };
        var result1 = await userManager.CreateAsync(user1, "Pass123$");
        result1.Succeeded.Should().BeTrue();

        var user2 = new ApplicationUser { UserName = "bob", Email = "bob2@example.com" };
        var result2 = await userManager.CreateAsync(user2, "Pass123$");

        result2.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task UserManager_RejectsWeakPassword()
    {
        using var dbFixture = new ApplicationDbContextFixture();
        var userManager = UserManagerFixture.Create(dbFixture);

        var user = new ApplicationUser { UserName = "charlie", Email = "charlie@example.com" };
        var result = await userManager.CreateAsync(user, "weak");

        result.Succeeded.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == "PasswordTooShort" || e.Code == "PasswordRequiresDigit" || e.Code == "PasswordRequiresUpper" || e.Code == "PasswordRequiresNonAlphanumeric");
    }
}
