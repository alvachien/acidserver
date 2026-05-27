using System.Data.Common;
using IdentityServerAspNetIdentity.Data;
using IdentityServerAspNetIdentity.Models;
using Microsoft.EntityFrameworkCore;

namespace AcidServer.Tests;

/// <summary>
/// Provides a fresh in-memory SQLite ApplicationDbContext for each test.
/// Uses a unique DbConnection per instance to ensure test isolation.
/// </summary>
public sealed class ApplicationDbContextFixture : IDisposable, IAsyncDisposable
{
    private readonly DbConnection _connection;

    public ApplicationDbContextFixture()
    {
        _connection = new Microsoft.Data.Sqlite.SqliteConnection("DataSource=:memory:");
        _connection.Open();
    }

    public DbConnection Connection => _connection;

    public ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    public void Dispose() => _connection.Dispose();
    public ValueTask DisposeAsync() => _connection.DisposeAsync();
}
