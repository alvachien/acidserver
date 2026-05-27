# Architecture Upgrade: Port .NET 10 Reference Project

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the current project's entire codebase with the reference project from `../../acidserver`, upgrading from .NET 6 to .NET 10, Duende.IdentityServer 6.0.4 to 7.4.7, classic hosting to minimal hosting, and scaffolded Identity UI to QuickStart Razor Pages UI.

**Architecture:** Full directory replacement — the reference project's source files become the current project. Git history of the current project is preserved; the change appears as a single large commit.

**Tech Stack:** .NET 10.0, Duende.IdentityServer 7.4.7, EF Core 10.0.7, Serilog 10.0.0, Razor Pages, ASP.NET Core Identity, SQL Server

---

### Context

**Current project** (`.NET 6`):
- Classic hosting: `Program.cs` + `Startup.cs` with `IHostBuilder`
- Identity UI: Scaffolded under `Areas/Identity/Pages/` (61 files)
- MVC: `Controllers/HomeController.cs` + `Views/` (7 files)
- Packages: Duende.IdentityServer 6.0.4, EF Core 6.0.3, Serilog 5.0.0
- User type: `IdentityUser` (no custom user class)
- 4 clients in Config.cs (HIH, Gallery, Knowledge Builder, Postman)
- Full bootstrap/jquery in `wwwroot/lib/`

**Reference project** (`.NET 10`):
- Minimal hosting: `Program.cs` (top-level statements) + `HostingExtensions.cs`
- Identity UI: QuickStart under `Pages/` (61 files: login, logout, consent, device, CIBA, grants, diagnostics)
- Pure Razor Pages (no MVC Controllers/Views)
- Packages: Duende.IdentityServer 7.4.7, EF Core 10.0.7, Serilog 10.0.0
- User type: `ApplicationUser : IdentityUser`
- 3 clients in Config.cs (no Postman)
- Minimal `wwwroot/` (favicon, duende-logo, bootstrap, glyphicons, jquery)
- Additional files: `SeedData.cs`, `SecurityHeadersAttribute.cs`, `Telemetry.cs`, `Extensions.cs`, `Log.cs`, `IdentityServerSuppressions.cs`, `buildschema.*`

### File Map

**Source (reference):** `D:\srccodes\achihui_gh\acidserver\..\..\acidserver`
**Target (current):** `D:\srccodes\achihui_gh\acidserver`

Files to copy (source files only, excluding build artifacts in `bin/`, `obj/`, `logs/`):

| Reference Path | Target Path | Notes |
|---|---|---|
| `.gitattributes` | `.gitattributes` | Replace |
| `.gitignore` | `.gitignore` | Replace |
| `LICENSE` | `LICENSE` | Replace (likely identical) |
| `README.md` | `README.md` | Replace |
| `acidserver.sln` | `acidserver.sln` | Replace |
| `src/acidserver/.config/dotnet-tools.json` | `src/acidserver/.config/dotnet-tools.json` | Replace |
| `src/acidserver/Config.cs` | `src/acidserver/Config.cs` | Replace |
| `src/acidserver/Data/ApplicationDbContext.cs` | `src/acidserver/Data/ApplicationDbContext.cs` | Replace |
| `src/acidserver/Data/Migrations/*` | `src/acidserver/Data/Migrations/*` | Replace |
| `src/acidserver/HostingExtensions.cs` | `src/acidserver/HostingExtensions.cs` | New |
| `src/acidserver/Models/ApplicationUser.cs` | `src/acidserver/Models/ApplicationUser.cs` | New |
| `src/acidserver/Pages/**` | `src/acidserver/Pages/**` | New (all 61 files) |
| `src/acidserver/Program.cs` | `src/acidserver/Program.cs` | Replace |
| `src/acidserver/Properties/launchSettings.json` | `src/acidserver/Properties/launchSettings.json` | Replace |
| `src/acidserver/SeedData.cs` | `src/acidserver/SeedData.cs` | New |
| `src/acidserver/acidserver.csproj` | `src/acidserver/acidserver.csproj` | Replace |
| `src/acidserver/appsettings.json` | `src/acidserver/appsettings.json` | Replace |
| `src/acidserver/buildschema.bat` | `src/acidserver/buildschema.bat` | New |
| `src/acidserver/buildschema.sh` | `src/acidserver/buildschema.sh` | New |
| `src/acidserver/wwwroot/**` | `src/acidserver/wwwroot/**` | Replace |

Files/directories to REMOVE (exist in current but not in reference):

| Path | Reason |
|---|---|
| `src/acidserver/Startup.cs` | Replaced by HostingExtensions.cs |
| `src/acidserver/Areas/` | Replaced by Pages/ |
| `src/acidserver/Controllers/` | No MVC |
| `src/acidserver/Views/` | No MVC |
| `src/acidserver/Models/ErrorViewModel.cs` | Replaced by Pages/Home/Error/ViewModel.cs |
| `src/acidserver/Properties/serviceDependencies*.json` | Not in reference |

---

### Task 1: Remove old source files from current project

**Files:** Remove the following from `D:\srccodes\achihui_gh\acidserver`:
- `src/acidserver/Startup.cs`
- `src/acidserver/Areas/` (entire directory)
- `src/acidserver/Controllers/` (entire directory)
- `src/acidserver/Views/` (entire directory)
- `src/acidserver/Models/ErrorViewModel.cs`
- `src/acidserver/Properties/serviceDependencies.json`
- `src/acidserver/Properties/serviceDependencies.local.json` (if exists)

- [ ] **Step 1: Remove old files and directories**

Run these commands:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
rm -rf src/acidserver/Startup.cs
rm -rf src/acidserver/Areas/
rm -rf src/acidserver/Controllers/
rm -rf src/acidserver/Views/
rm -rf src/acidserver/Models/ErrorViewModel.cs
rm -rf src/acidserver/Properties/serviceDependencies.json
rm -rf src/acidserver/Properties/serviceDependencies.local.json
```

Expected: All files/directories removed without errors.

- [ ] **Step 2: Verify removal**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
ls src/acidserver/Startup.cs 2>&1 || echo "OK: Startup.cs removed"
ls -d src/acidserver/Areas/ 2>&1 || echo "OK: Areas/ removed"
ls -d src/acidserver/Controllers/ 2>&1 || echo "OK: Controllers/ removed"
ls -d src/acidserver/Views/ 2>&1 || echo "OK: Views/ removed"
```

Expected: All should report "OK: removed".

---

### Task 2: Copy source files from reference project

**Files:** Copy all non-build-artifact source files from `../../acidserver` into current project.

**Excluded patterns (build artifacts):**
- `bin/` — compiled output
- `obj/` — build intermediates
- `logs/` — runtime log files
- `.git/` — git metadata
- `.claude/` — IDE settings
- `CLAUDE.md`, `REVIEW_FINDINGS.md` — reference project's dev docs

- [ ] **Step 1: Copy source files using rsync**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
rsync -av --delete \
  --exclude='.git/' \
  --exclude='.claude/' \
  --exclude='bin/' \
  --exclude='obj/' \
  --exclude='logs/' \
  --exclude='CLAUDE.md' \
  --exclude='REVIEW_FINDINGS.md' \
  "../acidserver/" ./
```

Wait — the reference is at `../../acidserver`, not `../acidserver`. Correct command:

```bash
cd "D:\srccodes\achihui_gh\acidserver"
rsync -av --delete \
  --exclude='.git/' \
  --exclude='.claude/' \
  --exclude='bin/' \
  --exclude='obj/' \
  --exclude='logs/' \
  --exclude='CLAUDE.md' \
  --exclude='REVIEW_FINDINGS.md' \
  "../../acidserver/" ./
```

Expected: Files copied, old files replaced, new files added. Output shows transferred files.

If rsync is not available on Windows, use xcopy or robocopy alternative:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
# Use cp -r as fallback (Git Bash on Windows)
find "../../acidserver" -not -path '*/.git/*' -not -path '*/.git' -not -path '*/.claude/*' -not -path '*/bin/*' -not -path '*/obj/*' -not -path '*/logs/*' -not -name 'CLAUDE.md' -not -name 'REVIEW_FINDINGS.md' -type f | while read f; do
  rel="${f#../../acidserver/}"
  dir=$(dirname "$rel")
  mkdir -p "$dir"
  cp "$f" "$rel"
done
```

- [ ] **Step 2: Verify file counts match**

Run:
```bash
# Count source files in reference (excluding build artifacts)
ref_count=$(find "../../acidserver" -not -path '*/.git/*' -not -path '*/.git' -not -path '*/.claude/*' -not -path '*/bin/*' -not -path '*/obj/*' -not -path '*/logs/*' -not -name 'CLAUDE.md' -not -name 'REVIEW_FINDINGS.md' -type f | wc -l)

# Count source files in current (excluding build artifacts)
cur_count=$(find . -not -path '*/.git/*' -not -path '*/.git' -not -path '*/.claude/*' -not -path '*/bin/*' -not -path '*/obj/*' -not -path '*/logs/*' -not -name 'CLAUDE.md' -not -name 'REVIEW_FINDINGS.md' -type f | wc -l)

echo "Reference: $ref_count files"
echo "Current: $cur_count files"
```

Expected: Both counts should be equal.

- [ ] **Step 3: Verify content matches**

Run:
```bash
diff -rq --exclude=.git --exclude=.claude --exclude=bin --exclude=obj --exclude=logs --exclude=CLAUDE.md --exclude=REVIEW_FINDINGS.md "../../acidserver" .
```

Expected: No output (directories are identical).

---

### Task 3: Verify new project structure

**Files:** Key files to verify exist and have correct content.

- [ ] **Step 1: Verify key new files exist**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
for f in \
  src/acidserver/HostingExtensions.cs \
  src/acidserver/SeedData.cs \
  src/acidserver/Models/ApplicationUser.cs \
  src/acidserver/Pages/Index.cshtml \
  src/acidserver/Pages/Index.cshtml.cs \
  src/acidserver/Pages/Account/Login/Index.cshtml \
  src/acidserver/Pages/Account/Logout/Index.cshtml \
  src/acidserver/Pages/Consent/Index.cshtml \
  src/acidserver/Pages/Device/Index.cshtml \
  src/acidserver/Pages/Ciba/Index.cshtml \
  src/acidserver/Pages/Grants/Index.cshtml \
  src/acidserver/Pages/Diagnostics/Index.cshtml \
  src/acidserver/buildschema.bat \
  src/acidserver/buildschema.sh; do
  if [ -f "$f" ]; then echo "OK: $f"; else echo "MISSING: $f"; fi
done
```

Expected: All should report "OK".

- [ ] **Step 2: Verify old files are gone**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
for f in \
  src/acidserver/Startup.cs \
  src/acidserver/Models/ErrorViewModel.cs; do
  if [ ! -e "$f" ]; then echo "OK: $f removed"; else echo "STILL EXISTS: $f"; fi
done
for d in \
  src/acidserver/Areas \
  src/acidserver/Controllers \
  src/acidserver/Views; do
  if [ ! -d "$d" ]; then echo "OK: $d removed"; else echo "STILL EXISTS: $d"; fi
done
```

Expected: All should report "OK: removed" or "OK: removed".

- [ ] **Step 3: Verify csproj targets .NET 10**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
grep -o 'net[0-9]*' src/acidserver/acidserver.csproj
```

Expected: Output should contain `net10.0`.

---

### Task 4: Attempt build to verify project compiles

**Prerequisites:** .NET 10 SDK must be installed.

- [ ] **Step 1: Check .NET SDK version**

Run:
```bash
dotnet --list-sdks
```

Expected: Should include `10.0.x`. If not available, note this as a blocker and skip the build step.

- [ ] **Step 2: Restore packages**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
dotnet restore src/acidserver/acidserver.csproj
```

Expected: Success, packages restored.

- [ ] **Step 3: Build the project**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
dotnet build src/acidserver/acidserver.csproj --no-restore
```

Expected: Build succeeds with 0 errors. Warnings are acceptable.

- [ ] **Step 4: If build fails, diagnose**

If the build fails, capture the error output and check:
- Are any required NuGet packages missing from csproj?
- Are there namespace conflicts (reference uses `IdentityServerAspNetIdentity`, current may have leftover references)?
- Is the .NET 10 SDK actually installed?

---

### Task 5: Commit the port

- [ ] **Step 1: Review git status**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
git status
```

Expected: Shows many deletions (old files) and additions (new files). `.git/` itself should not be affected.

- [ ] **Step 2: Stage all changes**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
git add -A
```

- [ ] **Step 3: Commit**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
git commit -m "$(cat <<'EOF'
chore: upgrade to .NET 10 with Duende IdentityServer 7.4 QuickStart UI

Replace entire codebase with reference project:
- .NET 6 → .NET 10
- Classic hosting (Startup.cs) → minimal hosting (HostingExtensions.cs)
- Scaffolded Identity UI (Areas/Identity/) → QuickStart Razor Pages (Pages/)
- Duende.IdentityServer 6.0.4 → 7.4.7
- EF Core 6.0.3 → 10.0.7
- IdentityUser → ApplicationUser
- 4 clients → 3 clients (removed Postman)
- Add SeedData.cs, SecurityHeadersAttribute.cs, Telemetry.cs, Extensions.cs

Co-Authored-By: Claude Opus 4.7 <noreply@anthropic.com>
EOF
)"
```

- [ ] **Step 4: Verify commit**

Run:
```bash
cd "D:\srccodes\achihui_gh\acidserver"
git log --oneline -1
git diff --stat HEAD~1..HEAD | tail -1
```

Expected: Shows the commit message and a summary of files changed.

---

## Spec Self-Review

1. **Spec coverage:**
   - Framework upgrade .NET 6 → .NET 10: Task 2 (csproj copy), Task 3 Step 3 (verify)
   - Hosting model change: Task 2 (Program.cs, HostingExtensions.cs copy), Task 3 Step 1 (verify)
   - Identity UI replacement: Task 1 (remove Areas/), Task 2 (copy Pages/)
   - MVC removal: Task 1 (remove Controllers/, Views/)
   - Package upgrade: Task 2 (csproj copy)
   - User type change: Task 2 (ApplicationUser.cs copy)
   - Config change: Task 2 (appsettings.json, Config.cs copy)
   - Static assets: Task 2 (wwwroot/ copy, Task 1 implicitly removes old via rsync --delete)
   - Git preservation: Task 5 (commit with preserved history)
   - SeedData, utilities: Task 2 (copy), Task 3 Step 1 (verify)
   - Build verification: Task 4
   - All covered.

2. **Placeholder scan:** No TBD, TODO, or vague instructions. All steps have actual commands.

3. **Type consistency:** File paths are consistent throughout. No conflicting references.

4. **Scope check:** This is a single operation (directory replacement) broken into logical phases. Focused.
