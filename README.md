# acidserver

Identity and authentication server for the HIH ecosystem and related applications, built on **Duende IdentityServer 7** with **ASP.NET Core Identity** and **ASP.NET Core 10.0**.

acidserver issues JWT access tokens via OpenID Connect (OIDC) to three client applications and protects access to their corresponding APIs.

## Role in the monorepo

acidserver is the first service that must be running — both `achihapi` (the OData API) and `achihui` (the Angular front-end) depend on it for authentication.

```
acidserver  →  achihapi  →  achihui
(ID server)    (OData API)   (Angular UI)
```

Use the root [`start-all.ps1`](../start-all.ps1) script to launch all three services in order.

## Tech stack

| Component | Technology |
|---|---|
| Runtime | .NET 10.0 |
| Framework | ASP.NET Core |
| Identity | Duende IdentityServer 7.4.7 + ASP.NET Core Identity |
| Database | SQLite via EF Core 10.0 (`acidserver.db`) |
| Logging | Serilog (file + console) |
| Testing | xUnit, FluentAssertions, coverlet |

## Project structure

```
acidserver/
├── acidserver.sln
├── src/acidserver/
│   ├── Program.cs                  # Entry point, Serilog bootstrap, DB creation
│   ├── HostingExtensions.cs        # Service registration & middleware pipeline
│   ├── Config.cs                   # OIDC resources, API scopes, client definitions
│   ├── ClientConfig.cs             # Configuration models for clients & CORS
│   ├── SeedData.cs                 # Development user seeding (alice / bob)
│   ├── ProfileService.cs           # Custom IProfileService (injects "name" claim)
│   ├── Data/
│   │   ├── ApplicationDbContext.cs  # EF Core context for ASP.NET Identity
│   │   └── Migrations/
│   ├── Models/
│   │   └── ApplicationUser.cs      # Custom Identity user
│   ├── Pages/                      # Razor Pages (login, logout, consent, CIBA, etc.)
│   ├── Properties/
│   │   └── launchSettings.json     # Dev URL: https://localhost:7228
│   ├── appsettings.json            # Connection strings, Serilog, client config
│   ├── appsettings.Development.json
│   └── appsettings.Production.json
├── tests/acidserver.Tests/         # xUnit tests (EF Core in-memory SQLite)
├── docs/superpowers/               # Design specs & implementation plans
├── .github/workflows/
│   └── dotnet-tests.yml            # CI: build + test on .NET 10.0.x
└── LICENSE
```

## Configuration

### `appsettings.json`

The `AcidServer` section defines the OIDC clients and allowed CORS origins. Each client entry specifies its `ClientId`, redirect URIs, and post-logout redirect URIs:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=acidserver.db"
  },
  "AcidServer": {
    "Clients": [
      {
        "ClientId": "achihui.js",
        "RedirectUris": [ "https://localhost:29521/signin-callback" ],
        "PostLogoutRedirectUris": [ "https://localhost:29521" ]
      }
    ],
    "AllowedCorsOrigins": [
      "https://localhost:29521",
      "https://localhost:29528",
      "https://localhost:44360"
    ]
  }
}
```

### OIDC clients

Three clients are configured in [`Config.cs`](src/acidserver/Config.cs), all using **Authorization Code** flow with **PKCE**:

| Client name | Client ID | API scope | Default token lifetime |
|---|---|---|---|
| AC HIH App | `achihui.js` | `api.hih` | 3600 s |
| AC Photo Gallery | `acgallery.app` | `api.acgallery` | 3600 s |
| Knowledge Builder | `knowledgebuilder.js` | `api.knowledgebuilder` | 3600 s |

All clients share these settings:
- `RequirePkce = true`, `RequireClientSecret = false`
- `AllowOfflineAccess = true` (refresh tokens, one-time use only)
- `AlwaysIncludeUserClaimsInIdToken = true`
- `RequireConsent = false`
- Scopes: `openid`, `profile`, `email`, `offline_access`, plus the client-specific API scope

### Identity resources

- `openid`, `profile`, `email`

### API resources & scopes

| API resource | Scope | User claims |
|---|---|---|
| `api.hih` — HIH API | `api.hih` | `name` |
| `api.acgallery` — Gallery API | `api.acgallery` | `name` |
| `api.knowledgebuilder` — Knowledge Builder API | `api.knowledgebuilder` | `name` |

## Build & run

### Build

```bash
cd acidserver
dotnet build src/acidserver/acidserver.csproj
```

### Run (development)

```bash
dotnet run --project src/acidserver/acidserver.csproj
```

The server starts at **`https://localhost:7228`** (configured in `launchSettings.json`).

On first startup, `Database.EnsureCreated()` creates the SQLite database (`acidserver.db`) automatically — no manual migration steps are required.

### Run the full stack

From the repository root:

```powershell
.\start-all.ps1              # fail if any port is occupied
.\start-all.ps1 -ForceKill   # kill whatever is on the port first
```

## Test

```bash
dotnet test tests/acidserver.Tests/acidserver.Tests.csproj
```

Tests use xUnit with in-memory SQLite and an auto-created database (see `ApplicationDbContextFixture`).

## CI

GitHub Actions workflow [`.github/workflows/dotnet-tests.yml`](.github/workflows/dotnet-tests.yml) runs restore → build → test on every push and pull request to `main`, using .NET 10.0.x on Ubuntu.

## Middleware pipeline

Configured in [`HostingExtensions.cs`](src/acidserver/HostingExtensions.cs):

1. Serilog request logging
2. Developer exception page (Development only)
3. CORS (origins from `AcidServer:AllowedCorsOrigins`)
4. Static files
5. Routing
6. IdentityServer
7. Authorization
8. Razor Pages (requires authorization)

## Seed data

[`SeedData.cs`](src/acidserver/SeedData.cs) provides two development users for local testing. Call `SeedData.EnsureSeedData(app)` from `Program.cs` when needed:

| User | Password | Claims |
|---|---|---|
| `alice` | `Pass123$` | name: Alice Smith, given_name: Alice, family_name: Smith |
| `bob` | `Pass123$` | name: Bob Smith, given_name: Bob, family_name: Smith, location: somewhere |

> **Note:** `SeedData` is not called automatically in the current `Program.cs` — the database is created but left empty. Invoke it manually during development if you need test accounts.

## Build configurations

| Configuration | Description |
|---|---|
| `Debug` | Standard development build |
| `Release` | Production build |
| `DeliverToCloud` | Cloud deployment build (defines `DEBUG` constant) |

## License

[MIT](LICENSE)

## Author

**Alva Chien (钱红俊)** — <alvachien@163.com> · <https://www.alvachien.com>
