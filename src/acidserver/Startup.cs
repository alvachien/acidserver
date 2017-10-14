
#define USE_MICROSOFTAZURE
//#define USE_ALIYUN

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using acidserver.Configuration;
using acidserver.Data;
using acidserver.Models;
using acidserver.Services;

namespace acidserver
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // Add framework services.
#if DEBUG
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("DebugConnection")));
                options.UseSqlServer(Configuration["ConnectionStrings:DebugConnection"]));
            
#elif USE_MICROSOFTAZURE
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AzureConnection")));
#elif USE_ALIYUN
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AliyunConnection")));
#endif

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = "998042782978-s07498t8i8jas7npj4crve1skpromf37.apps.googleusercontent.com";
                    options.ClientSecret = "HsnwJri_53zn7VcO1Fm7THBb";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
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
                    "https://localhost:20000",

                    "http://localhost:54020", // AC Quiz API
                    "https://localhost:54020"

#elif USE_MICROSOFTAZURE
                    "http://achihui.azurewebsites.net", 
                    "https://achihui.azurewebsites.net", 

                    "http://achihapi.azurewebsites.net",
                    "https://achihapi.azurewebsites.net",

                    "http://acgallery.azurewebsites.net",
                    "https://acgallery.azurewebsites.net",

                    "http://acgalleryapi.azurewebsites.net",                    
                    "https://acgalleryapi.azurewebsites.net",

                    "http://acmathexercise.azurewebsites.net",                    
                    "https://acmathexercise.azurewebsites.net",

                    "http://acquizapi.azurewebsites.net",                    
                    "https://acquizapi.azurewebsites.net"
#elif USE_ALIYUN
                    "http://118.178.58.187:5200", // HIH UI
                    "https://118.178.58.187:5200",

                    "http://118.178.58.187:5210", // Gallery
                    "https://118.178.58.187:5210",

                    "http://118.178.58.187:5230", // Math exercise
                    "https://118.178.58.187:5230",

                    "http://118.178.58.187:5300", // HIH API
                    "https://118.178.58.187:5300",

                    "http://118.178.58.187:5310", // Gallery API
                    "https://118.178.58.187:5310",

                    "http://118.178.58.187:5330", // Quiz API
                    "https://118.178.58.187:5330"
#endif
                    )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
            app.UseStaticFiles();

            // app.UseIdentity(); // not needed, since UseIdentityServer adds the authentication middleware
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
