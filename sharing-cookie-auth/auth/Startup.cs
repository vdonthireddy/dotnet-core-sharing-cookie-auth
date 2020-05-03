using System;
using System.IO;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace auth
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
                    config.LoginPath = "/auth/login";
                    config.Cookie.Domain = "localhost";
                    config.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                });

            services.AddDataProtection()
                .PersistKeysToFileSystem(GetKyRingDirectoryInfo())
                .SetApplicationName("SharedCookieApp");
        }

        private DirectoryInfo GetKyRingDirectoryInfo()
        {
            string keyRingPath = Configuration.GetSection("AppKeys").GetValue<string>("keyRingPath");
            DirectoryInfo keyRingDirectoryInfo = new DirectoryInfo($"{keyRingPath}");
            if (!keyRingDirectoryInfo.Exists)
            {
                Directory.CreateDirectory(keyRingDirectoryInfo.FullName);
            }
            return keyRingDirectoryInfo;
        }

        //private DirectoryInfo GetKyRingDirectoryInfo()
        //{
        //    string applicationBasePath = System.AppContext.BaseDirectory;
        //    DirectoryInfo directoryInof = new DirectoryInfo(applicationBasePath);
        //    string keyRingPath = Configuration.GetSection("AppKeys").GetValue<string>("keyRingPath");
        //    do
        //    {
        //        directoryInof = directoryInof.Parent;

        //        DirectoryInfo keyRingDirectoryInfo = new DirectoryInfo($"{directoryInof.FullName}{keyRingPath}");
        //        if (keyRingDirectoryInfo.Exists)
        //        {
        //            return keyRingDirectoryInfo;
        //        }

        //    }
        //    while (directoryInof.Parent != null);
        //    throw new Exception($"key ring path not found");
        //}

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
            app.UseHttpsRedirection();
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
