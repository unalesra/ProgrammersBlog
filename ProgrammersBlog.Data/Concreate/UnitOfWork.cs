using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Data.Concreate.EntityFramework.Contexts;
using ProgrammersBlog.Data.Concreate.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Data.Concreate
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProgrammersBlogContext _context;

        private EfArticleRepository _articleRepository;
        private EfCategoryRepository _categoryRepository;
        private EfCommentRepository _commentRepository;
        private EfRoleRepository _roleRepository;
        private EfUserRepository _userRepository;

        public UnitOfWork(ProgrammersBlogContext context)
        {
            _context = context;
        }

        public async ValueTask IAsyncDisposable()
        {
            await _context.DisposeAsync();
        }

        //?? null coalescing
        //eğer değer null gelmişse şu işlemi yap 
        //int x ?? x=10 gibi
        public IArticleRepository Articles => _articleRepository ?? new EfArticleRepository(_context); //_article repository'i döndür diyorum. somut halini verdim. Eğer nesne oluşturulmamışsa oluştur demiş oldum ?? ile

        public ICategoryRepository Categories => _categoryRepository ?? new EfCategoryRepository(_context);

        public ICommentRepository Comments =>_commentRepository ?? new EfCommentRepository(_context);

        public IRoleRepository Roles => _roleRepository ?? new EfRoleRepository(_context);

        public IUserRepository Users => _userRepository ?? new EfUserRepository(_context);

        public async Task<int> SaveAsync()
        {
            //burada int dememe gerek yok savechanges dediğimde zaten int değeriyle beraber döndürüyor
            return await _context.SaveChangesAsync();
        }
    }
}
