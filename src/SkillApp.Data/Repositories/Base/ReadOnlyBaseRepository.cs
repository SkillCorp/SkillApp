using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using SkillApp.Data.Interfaces.Base;
using SkillApp.Entities.Base;

namespace SkillApp.Data.Repositories.Base
{
    public abstract class ReadOnlyBaseRepository<T> : IReadOnlyRepository<T> where T : BaseEntity
    {
        private readonly MyDbContext _myDbContext;

        protected ReadOnlyBaseRepository(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        protected MyDbContext MyDbContext
        {
            get { return _myDbContext; }
        }

        public int Count()
        {
            return Get().Count();
        }
        public int Count(Expression<Func<T, bool>> where)
        {
            return Get().Count(where);
        }
        public async Task<int> CountAsync()
        {
            return await Get().CountAsync();
        }

        public bool Exists(Expression<Func<T, bool>> where)
        {
            return Get().Any(where);
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> where)
        {
            return await Get().AnyAsync(where);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            return await Get(includes).Where(where).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            return Get(includes);
        }

        public IQueryable<T> GetBy(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            return Get(includes).Where(where);
        }

        protected IQueryable<T> Get()
        {
            return MyDbContext.Set<T>().AsQueryable();
        }
        private IQueryable<T> Get<TProperty>(params Expression<Func<T, TProperty>>[] includes)
        {
            var query = MyDbContext.Set<T>().AsQueryable();

            if (includes != null && includes.Length > 0)
            {
                query = includes.Aggregate(query, (current, item) => current.Include(item));
            }
            return query;
        }
    }
}
