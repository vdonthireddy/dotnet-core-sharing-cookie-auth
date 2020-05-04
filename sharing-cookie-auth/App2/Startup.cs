using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace App2
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
            services.AddControllersWithViews();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(config =>
                {
                    config.Cookie.Name = "vj_auth_local";
                    config.Cookie.Domain = "localhost";
                    config.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                    config.Events = new CookieAuthenticationEvents()
                    {
                        OnRedirectToLogin = (context) =>
                        {
                            string returnUri = (context.Request.IsHttps) ? "https://" : "http://"
                                + context.Request.Host
                                + context.Request.Path;
                            string loginUrl = Configuration.GetValue<string>("LoginUrl");
                            context.HttpContext.Response.Redirect($"{loginUrl}?returnUri={returnUri}");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddDataProtection()
                .PersistKeysToFileSystem(GetKyRingDirectoryInfo())
                .SetApplicationName("SharedCookieApp");

            services.AddAntiforgery();
        }

        private DirectoryInfo GetKyRingDirectoryInfo()
        {
            string keyRingPath = string.Empty;

            keyRingPath = Environment.GetEnvironmentVariable("KEY_RING_PATH");

            if (string.IsNullOrEmpty(keyRingPath))
                keyRingPath = Configuration.GetSection("AppKeys").GetValue<string>("keyRingPath");

            Console.WriteLine("Key Ring Path: " + keyRingPath);

            DirectoryInfo keyRingDirectoryInfo = new DirectoryInfo($"{keyRingPath}");
            if (!keyRingDirectoryInfo.Exists)
            {
                Directory.CreateDirectory(keyRingDirectoryInfo.FullName);
            }
            return keyRingDirectoryInfo;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
