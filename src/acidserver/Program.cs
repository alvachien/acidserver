using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

// Config the log
builder.Host.UseSerilog((context, config) =>
{
    var environment = context.HostingEnvironment;
    var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}";

    //config.MinimumLevel.Is(environment.IsDevelopment() ? LogEventLevel.Information : LogEventLevel.Warning)
    //     .Enrich.FromLogContext()
    //     .WriteTo.File(
    //         path: "../Logs/ACIDServer/log-.txt",
    //         rollingInterval: RollingInterval.Day, // 按天滚动
    //         outputTemplate: outputTemplate,
    //         retainedFileCountLimit: 14 // 保留最近7天日志
    //     );
    if (environment.IsDevelopment())
    {
        config.MinimumLevel.Is(LogEventLevel.Information)
             .Enrich.FromLogContext()
             .WriteTo.Console(theme: SystemConsoleTheme.Colored);

    }
    else if (environment.IsProduction())
    {
        config.MinimumLevel.Is(LogEventLevel.Warning)
             .Enrich.FromLogContext()
             .WriteTo.File(
                 path: "../Logs/ACIDServer/log-.txt",
                 rollingInterval: RollingInterval.Day, // 按天滚动
                 outputTemplate: outputTemplate,
                 retainedFileCountLimit: 14 // 保留最近7天日志
             );
    }
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddCors();

builder.Services.AddIdentityServer(options => {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;

        // see https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
        options.EmitStaticAudienceClaim = true;
    })
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddAspNetIdentity<IdentityUser>();

builder.Services.AddAuthentication();

// Build the application
var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else if(builder.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

if (builder.Environment.IsDevelopment())
{
    app.UseCors(option =>
    {
        option.WithOrigins(
            "https://localhost:16001", // AC gallery
            "https://localhost:29521", // AC HIH UI
            "https://localhost:29528", // AC HIH App
            "https://localhost:44366", // AC HIH API
            "https://localhost:25325", // AC Gallery API
            "https://localhost:44367"    // Knowledge builder
            )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
}
else if (builder.Environment.IsProduction())
{
    app.UseCors(option =>
    {
        option.WithOrigins("https://www.alvachien.com")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
}

app.UseStaticFiles();
app.UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        endpoints.MapRazorPages();
    });
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.Run();

