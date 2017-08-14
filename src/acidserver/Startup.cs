
#define USE_MICROSOFTAZURE
//#define USE_ALIYUN

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using acidserver.Data;
using acidserver.Models;
using acidserver.Services;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4;
using acidserver.Configuration;

namespace acidserver
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                //builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            var cert = new X509Certificate2(Path.Combine(Environment.ContentRootPath, "idsrvtest.dat"), "idsrv3test");
            //var builder = services.AddIdentityServer(options =>
            //{
            //    options.AuthenticationOptions.AuthenticationScheme = "Cookies";
            //})
            //.AddInMemoryClients(Clients.Get())
            //.AddInMemoryScopes(Scopes.Get())
            //.SetSigningCredential(cert);


            // Add framework services.
#if DEBUG
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DebugConnection")));
#else
#if USE_MICROSOFTAZURE
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AzureConnection")));
#elif USE_ALIYUN
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AliyunConnection")));
#endif
#endif

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            //services.AddIdentityServer(options =>
            //    {
            //        options.AuthenticationOptions.AuthenticationScheme = "Cookies";
            //    })
            services.AddIdentityServer()
                .AddSigningCredential(cert)
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>()                
                .AddProfileService<AspIdProfileService>()
                ;
                //.SetSigningCredential(cert);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors(option =>
            {
                option.WithOrigins(
#if DEBUG
                    "http://localhost:1601", // AC gallery
                    "https://localhost:1601",

                    "http://localhost:29521", // AC HIH UI
                    "https://localhost:29521",

                    "http://localhost:25688",  // AC HIH API
                    "https://localhost:25688",

                    "http://localhost:25325",  // AC Gallery API
                    "https://localhost:25325",

                    "http://localhost:20000",  // Math exercise
                    "https://localhost:20000"
#else
#if USE_MICROSOFTAZURE
                    "http://achihui.azurewebsites.net", 
                    "https://achihui.azurewebsites.net", 

                    "http://achihapi.azurewebsites.net",
                    "https://achihapi.azurewebsites.net",

                    "http://acgallery.azurewebsites.net",
                    "https://acgallery.azurewebsites.net",

                    "http://acgalleryapi.azurewebsites.net",                    
                    "https://acgalleryapi.azurewebsites.net",

                    "http://acmathexercise.azurewebsites.net",                    
                    "https://acmathexercise.azurewebsites.net"
#elif USE_ALIYUN
                    "http://118.178.58.187:5200/",
                    "https://118.178.58.187:5200/",

                    "http://118.178.58.187:5210/",
                    "https://118.178.58.187:5210/",

                    "http://118.178.58.187:5300/",
                    "https://118.178.58.187:5300/",

                    "http://118.178.58.187:5310/",
                    "https://118.178.58.187:5310/",

                    "http://118.178.58.187:5320/",
                    "https://118.178.58.187:5320/"
#endif
#endif
                    )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();               
            });

            app.UseStaticFiles();

            app.UseIdentity();
            app.UseIdentityServer();

            //app.UseGoogleAuthentication(new GoogleOptions
            //{
            //    AuthenticationScheme = "Google",
            //    DisplayName = "Google",
            //    SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

            //    ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com",
            //    ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo"
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
