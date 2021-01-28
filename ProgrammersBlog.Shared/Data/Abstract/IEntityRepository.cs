using ProgrammersBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Shared.Data.Abstract
{
    public interface IEntityRepository<T> where T:class,IEntity,new()
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties); //async çalışmasını istediğim için Task dedim.
        //Expression<Func<T,bool>> predicate dediğimiz şey
        /*
         var makale=repository.Get(m=>m.Id==15)
        var kişi= repository.Get(m=>m.Name=="Alper")

        15, alper gibi verdiğimiz lambda expressionlar predicate'dir.
        */

        //PARAMS EXPRESSION Expression<Func<T, object>>[] includeProperties
        /*
         kullanıcıyla beraber onun yorumlarını da getirmek istediğim zaman kullanıyorum.
        var makale=rpository.GetAsync(m=>m.Id==25, m=>m.User, m=>m.Comments);

        istediğim kadar property verebilirim, yani istediğim kadar include yapabiliyorum.
        burada predicate değeri null gelemez.
         */

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
        //eğer parametre gelmemişse hepsini listele

        Task AddAsync(T entity);

        Task UpdateAsync(T Entity);

        Task DeleteAsync(T entity);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        /*
         * Daha önce böyle bir kullanıcı kayıt olmuş mu?
         */

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

    }
}
