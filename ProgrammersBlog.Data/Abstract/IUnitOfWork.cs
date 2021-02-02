using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Data.Abstract
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IArticleRepository Articles { get; }
        ICategoryRepository Categories { get; }
        ICommentRepository Comments { get; }
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }

        //veritabanına işlemleri kaydetmek
        //burada eklememin sebepler
        /*
         * 1-bir sürü ekleme işlemi yapsam da tek save ile işi çözebileceğim
         * 2-eğer eklemelerin birinde hata oluşursa, bu veriler veritabanına hiç ulaşamamış olacaklar. Böylece veritabanına eksik bilgi gitmemiş olacak.
         */
        Task<int> SaveAsync();

    }
}
