using Estore.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Estore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(config =>
            {
                config.Cookie.IsEssential = true;
                config.Cookie.SameSite = SameSiteMode.Strict;
                config.LoginPath = "/signin";
            });
            services.AddDbContext<EstoreDbContext>(options
                => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddControllersWithViews();
            services.AddSession();
            services.AddRouting(options => options.LowercaseUrls = true);
        }
            
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapControllerRoute(
                    name: "item",
                    pattern: "item/{findcode}",
                    defaults: new {controller = "Item", action = "Index"}
                );
                endpoints.MapControllerRoute(
                    name: "signout",
                    pattern: "/signout",
                    defaults: new {controller = "Home", action = "Leave"}
                );
            });
        }
    }
}