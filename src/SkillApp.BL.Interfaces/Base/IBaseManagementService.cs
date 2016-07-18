using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SkillApp.BL.Interfaces.Base
{
    public interface IBaseManagementService<T>
    {
        IQueryable<T> GetAll();
        void Create(T entity);
        Task CreateAsync(T entity);
        void Create(ICollection<T> entities);
        Task CreateAsync(ICollection<T> entities);
        T CreateAndReturn(T entity);
        Task<T> CreateAndReturnAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        void Update(ICollection<T> entities);
        Task UpdateAsync(ICollection<T> entities);
        void Delete(Expression<Func<T, bool>> where);
        Task DeleteAsync(Expression<Func<T, bool>> where);
    }
}
