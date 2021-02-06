using Microsoft.Extensions.DependencyInjection;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Data.Concreate;
using ProgrammersBlog.Data.Concreate.EntityFramework.Contexts;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Services.Extensions
{
    public static class ServiceCollectionsExtensions
    {
        //servis katmanı ile mvc katmanı arasınaki bağlantı için yazıldı
        public static IServiceCollection LoadMyService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ProgrammersBlogContext>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();

            return serviceCollection;

        }
    }
}
