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
using acidserver.Data;
using acidserver.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace acidserver
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });

            services.AddCors();

            // Add framework services.
#if DEBUG
            services.AddDbContext<ApplicationDbContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("DebugConnection")));
                options.UseSqlServer(Configuration.GetConnectionString("DebugConnection")));
#endif
#if RELEASE
#if USE_AZURE
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

            // configure identity server with in-memory stores, keys, clients and scopes
            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddDeveloperSigningCredential();

            builder.AddDeveloperSigningCredential();

            services.AddAuthentication();
                //.AddGoogle(options =>
                //{
                //    options.ClientId = "998042782978-s07498t8i8jas7npj4crve1skpromf37.apps.googleusercontent.com";
                //    options.ClientSecret = "HsnwJri_53zn7VcO1Fm7THBb";
                //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseCors(option =>
            {
                option.WithOrigins(
#if DEBUG
#if USE_SSL
                    "https://localhost:16001", // AC gallery
                    "https://localhost:29521", // AC HIH UI
                    "https://localhost:44366", // AC HIH API
                    "https://localhost:25325", // AC Gallery API
                    "https://localhost:20000", // Math exercise
                    "https://localhost:54020" // AC Quiz API
#else
                    "http://localhost:16001", // AC gallery
                    "http://localhost:29521", // AC HIH UI
                    "http://localhost:25688",  // AC HIH API
                    "http://localhost:25325",  // AC Gallery API
                    "http://localhost:20000",  // Math exercise
                    "http://localhost:54020"  // AC Quiz API
#endif
#endif

#if RELEASE
#if USE_AZURE
#if USE_SSL
                    "https://achihui.azurewebsites.net", 
                    "https://achihapi.azurewebsites.net",
                    "https://acgallery.azurewebsites.net",
                    "https://acgalleryapi.azurewebsites.net",
                    "https://acmathexercise.azurewebsites.net",
                    "https://acquizapi.azurewebsites.net"
#else
                    "http://achihui.azurewebsites.net",
                    "http://achihapi.azurewebsites.net",
                    "http://acgallery.azurewebsites.net",
                    "http://acgalleryapi.azurewebsites.net",
                    "http://acmathexercise.azurewebsites.net",
                    "http://acquizapi.azurewebsites.net"
#endif
#elif USE_ALIYUN
#if USE_SSL
                    "https://www.alvachien.com/hih", // HIH UI
                    "https://www.alvachien.com/gallery", // Gallery
                    "https://www.alvachien.com/math", // Math exercise
                    "https://www.alvachien.com/hihapi", // HIH API
                    "https://www.alvachien.com/galleryapi", // Gallery API
                    "https://www.alvachien.com/quizapi" // Quiz API
#else
                    "http://118.178.58.187:5200", // HIH UI
                    "http://118.178.58.187:5210", // Gallery
                    "http://118.178.58.187:5230", // Math exercise
                    "http://118.178.58.187:5300", // HIH API
                    "http://118.178.58.187:5310", // Gallery API
                    "http://118.178.58.187:5330" // Quiz API
#endif
#endif
#endif
                    )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
