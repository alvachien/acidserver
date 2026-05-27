---
name: architecture-upgrade-port
description: Port reference project (../../acidserver, .NET 10, Duende.IdentityServer 7.4, QuickStart UI) into current project (currently .NET 6, Duende.IdentityServer 6.0, scaffolded Identity UI)
metadata:
  type: project
---

# Architecture Upgrade: Port Reference Project

**Date:** 2026-05-27
**Source:** `../../acidserver` (.NET 10, Duende.IdentityServer 7.4.7, QuickStart UI)
**Target:** Current project (.NET 6, Duende.IdentityServer 6.0.4, scaffolded Identity UI)

## Goal

Replace current project's entire codebase with the reference project's structure, upgrading framework, packages, hosting model, and identity UI.

## Decisions

- **Approach:** Full directory replacement (copy reference files over current project, preserving `.git/`)
- **Framework:** .NET 6 → .NET 10
- **Hosting model:** `Startup.cs` + `IHostBuilder` → minimal hosting (`Program.cs` top-level + `HostingExtensions.cs`)
- **Identity UI:** Scaffolded `Areas/Identity/` → QuickStart `Pages/` (login, logout, consent, device flow, CIBA, grants, diagnostics)
- **MVC:** Removed (`Controllers/`, `Views/`) — pure Razor Pages
- **User type:** `IdentityUser` → `ApplicationUser : IdentityUser`
- **Packages:** Duende.IdentityServer 6.0.4 → 7.4.7, EF Core 6.0.3 → 10.0.7, Serilog 5.0.0 → 10.0.0
- **Config:** Reference project's `appsettings.json` (Serilog rolling file) and `Config.cs` (3 clients, no Postman)
- **Static assets:** Reference project's minimal `wwwroot/` (favicon, duende-logo.svg)
- **Git:** Preserve current project's `.git/` history; one commit for the port

## Files Removed (from current project)

- `Startup.cs` — replaced by `HostingExtensions.cs`
- `Areas/Identity/` (61 files) — replaced by `Pages/`
- `Controllers/HomeController.cs` — no MVC
- `Views/` (7 files) — no MVC
- `Models/ErrorViewModel.cs` — replaced by `Pages/Home/Error/ViewModel.cs`
- `wwwroot/lib/` (bootstrap/jquery/jquery-validation distributions) — not needed
- `Properties/serviceDependencies.json` — not needed

## Files Added (from reference project)

- `HostingExtensions.cs` — service configuration and pipeline setup
- `SeedData.cs` — database seeding (alice/bob test users)
- `Models/ApplicationUser.cs` — custom user entity
- `Pages/` (61 files) — QuickStart UI:
  - `Account/Login/` (5 files) — custom login flow
  - `Account/Logout/` (5 files) — custom logout flow
  - `Account/AccessDenied/` (2 files)
  - `Consent/` (5 files) — OAuth consent
  - `Ciba/` (7 files) — backchannel authentication
  - `Device/` (7 files) — device flow
  - `Diagnostics/` (3 files)
  - `ExternalLogin/` (4 files)
  - `Grants/` (3 files) — grant management
  - `ServerSideSessions/` (2 files)
  - `Redirect/` (2 files)
  - `Home/Error/` (3 files)
  - `Index.cshtml` + `.cs` — landing page
  - `Shared/` (3 files) — layout, nav, validation summary
- `Pages/SecurityHeadersAttribute.cs`
- `Pages/Telemetry.cs`
- `Pages/Extensions.cs`
- `Pages/Log.cs`
- `Pages/IdentityServerSuppressions.cs`
- `buildschema.bat`, `buildschema.sh`

## Files Replaced (updated versions from reference)

- `acidserver.csproj` — .NET 10, updated packages
- `Program.cs` — minimal hosting
- `Config.cs` — 3 clients, updated URIs
- `appsettings.json` — Serilog rolling file config
- `acidserver.sln` — updated solution
- `Data/ApplicationDbContext.cs` — uses `ApplicationUser`
- `Data/Migrations/` — updated for new schema
- `.gitignore`, `.gitattributes` — reference versions
