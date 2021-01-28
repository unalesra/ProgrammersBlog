using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Entities.Concreate;
using ProgrammersBlog.Shared.Data.Concreate.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Data.Concreate.EntityFramework.Repositories
{
    public class EfArticleRepository:EfRepositoryBase<Article>,IArticleRepository
    {

        public EfArticleRepository(DbContext context): base(context){}
    }
}
