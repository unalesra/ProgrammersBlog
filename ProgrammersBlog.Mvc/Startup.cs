using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammersBlog.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //bir mvc katmaný olduðunu gösterdik.
            //her önyüz deðiþikliðinde önyüzü derlemek zorunda kalmamak için addrazorruntimecompiilation ekledik.
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //derlenme esnasýnda automapper'in burada olan sýnýflarý taramasýný saðlýyoruz.
            services.AddAutoMapper(typeof(Startup));

            //servis katmaný ile mvc katmaný arasýnaki baðlantý
            services.LoadMyService();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //bulunamayan bir sayfaya gittiðinde 404 hatasý ver
                app.UseStatusCodePages();
            }

            //static dosyalarý kullan
            app.UseStaticFiles();


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //admin için route
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{Controller=Home}/{action=Index}/{id?}"
                    );


                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
