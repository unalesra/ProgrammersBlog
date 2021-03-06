﻿using Microsoft.Extensions.DependencyInjection;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Data.Concreate;
using ProgrammersBlog.Data.Concreate.EntityFramework.Contexts;
using ProgrammersBlog.Entities.Concreate;
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
            serviceCollection.AddIdentity<User, Role>(options=> {

                // User Password Options
                options.Password.RequireDigit = false;   //şifre içerisinde rakam olması zorunlu olsun mu olmasın mı
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;  //kaç farklı unique karakter olsun şifre içerisinde(iki tane @ kullanılırsa 1 unique karakter sayar) 
                options.Password.RequireNonAlphanumeric = false;  //özel karakter zorunlu mu değil mi
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                // User Username and Email Options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+$";
                options.User.RequireUniqueEmail = true;


            }).AddEntityFrameworkStores<ProgrammersBlogContext>();
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();

            return serviceCollection;

        }
    }
}
