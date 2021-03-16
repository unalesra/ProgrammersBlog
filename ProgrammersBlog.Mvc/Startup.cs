using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammersBlog.Services.AutoMapper.Profiles;
using ProgrammersBlog.Services.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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
            // json objelerinin dönüþtürlmesi için addjsonoptions geldi
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt=> {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                //iç içe olan objeler birbirini referans ettiðinde sorun olmuþmamasý için ekledik.
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });

            //session
            services.AddSession();

            //derlenme esnasýnda automapper'in burada olan sýnýflarý taramasýný saðlýyoruz.
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile));

            //servis katmaný ile mvc katmaný arasýnaki baðlantý
            services.LoadMyService();

            //cookie
            services.ConfigureApplicationCookie(options=> {

                //login yapýlýrken hangi sayfaya yönlendirileceðim?
                //admin area user controller login action
                options.LoginPath = new PathString("/Admin/User/Login");

                options.LogoutPath = new PathString("/Admin/User/Logout");

                options.Cookie = new CookieBuilder {
                    Name = "ProgrammersBlog",
                    HttpOnly = true,   //güvenlik için true veriyoruz. cookie iþlemlerini sadece http üzerinden göndermesini saðlýyoruz ve böylece javascript tarafýnda(ön yüz) kimse cookie bilgilerimize eriþemmeiþ oluyor.
                    SameSite = SameSiteMode.Strict,   //CSRF'yi önlemek için, cookie ilgileri sadece benim sitemden gelirse kullan dmeiþ olduk.
                    SecurePolicy= CookieSecurePolicy.SameAsRequest, //http üzeirnden gelirse htttp, https üzerinden gelirse https üzerinden bilgileri aktar demek ama gerçek uygulamalrda always olarak kullanýlýr yani ne olursa olsun https üzerinden bilgileri aktar demiþ oluruz. 
                };

                options.SlidingExpiration = true; //kullanýcý giriþ yaptýktan sonra ne kadar süre tanýnacak ve ne zaman tekrar giriþ yapmasý gerekecek?
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7);  //7 gün boyunca bir daha giriþ yapmasý gerekmesin
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied"); //sistem içerisinde zaten giriþ yapmýþ bir kullanýcý yetkisinin olmadýðý yere girerse buraya yönlendir.
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //BULUNAMAYAN BÝR SAYFAYA GÝTTÝÐÝNDE 404 HATASI VER
                app.UseStatusCodePages();
            }

            //session'un yeri de aþaðýdaki sebepten ötürü önemli
            app.UseSession();

            //static dosyalarý kullan
            app.UseStaticFiles();
            app.UseRouting();

            //routing yapýldýktan(kullanýncýnýn nereye gitmek istediðini öðrendikten) sonra authentication ve authorization kontrollerinin yapýlmasý gerekiyor daha önce yapamayýz.
            app.UseAuthentication();
            app.UseAuthorization();


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
