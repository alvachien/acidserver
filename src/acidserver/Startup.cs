using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
using Microsoft.AspNetCore.Http;
using acidserver.Configuration;
using IdentityServer4.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using IdentityServer4;

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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
#endif

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IProfileService, AspIdProfileService>();

            //services.AddIdentityServer(options =>
            //    {
            //        options.AuthenticationOptions.AuthenticationScheme = "Cookies";
            //    })
            services.AddDeveloperIdentityServer()
                .AddInMemoryScopes(Config.GetScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<AspIdProfileService>()                
                .SetSigningCredential(cert);
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
                    "https://localhost:25325"
#else
                    "http://achihui.azurewebsites.net", 
                    "http://achihapi.azurewebsites.net",
                    "http://acgallery.azurewebsites.net",
                    "https://achihui.azurewebsites.net", 
                    "https://achihapi.azurewebsites.net",
                    "https://acgallery.azurewebsites.net"
#endif
                    )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();               
            });

            app.UseStaticFiles();

            app.UseIdentity();
            app.UseIdentityServer();

            app.UseGoogleAuthentication(new GoogleOptions
            {
                AuthenticationScheme = "Google",
                DisplayName = "Google",
                SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme,

                ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com",
                ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo"
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
