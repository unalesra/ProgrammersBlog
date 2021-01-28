using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Shared.Data.Abstract;
using ProgrammersBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Shared.Data.Concreate.EntityFramework
{
    public class EfRepositoryBase<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity, new()
    {

        private readonly DbContext _context;

        public EfRepositoryBase(DbContext context)
        {
            this._context = context;
        }
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _context.Set<TEntity>().CountAsync(predicate);
        }

        //delete ve update'de async çalışma yok. Bu yüzden biz anonim tasklar oluşturduk.
        public async Task DeleteAsync(TEntity entity)
        {
            await Task.Run(() =>
            {
                _context.Set<TEntity>().Remove(entity);
            });
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (predicate != null) query = query.Where(predicate);
            if(includeProperties.Any()) //dizide herhangi bir değer var mı? varsa query'e includeları ekle.
            {
                foreach(var includePropery in includeProperties)
                {
                    query = query.Include(includePropery);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();
            if (predicate != null) query = query.Where(predicate);
            if (includeProperties.Any()) 
            {
                foreach (var includePropery in includeProperties)
                {
                    query = query.Include(includePropery);
                }
            }
            return await query.SingleOrDefaultAsync();
        }

        public async Task UpdateAsync(TEntity Entity)
        {
            await Task.Run(() =>
            {
                _context.Set<TEntity>().Update(Entity);
            });
        }
    }
}
