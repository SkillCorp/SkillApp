using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using SkillApp.BL.Interfaces.Base;
using SkillApp.Data.Interfaces.Base;
using SkillApp.Entities.Base;

namespace SkillApp.BL.Base
{
    public abstract class BaseManagementService<T, R> : IBaseManagementService<T>
       where R : IRepository<T>
       where T : BaseEntity
    {
        public virtual R Repository { get; protected set; }

        #region Get methods

        /// <summary>
        /// Gets all
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return Repository.GetAll();
        }

        #endregion Get methods

        #region Create methods

        /// <summary>
        ///     Creates entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Create(T entity)
        {
            Repository.Insert(entity);
            Repository.SaveDbChanges();
        }
        public async Task CreateAsync(T entity)
        {
            Repository.Insert(entity);
            await Repository.SaveDbChangesAsync();
        }

        public virtual T CreateAndReturn(T entity)
        {
            Repository.Insert(entity);
            Repository.SaveDbChanges();
            return entity;
        }
        public async Task<T> CreateAndReturnAsync(T entity)
        {
            Repository.Insert(entity);
            await Repository.SaveDbChangesAsync();
            return entity;
        }

        /// <summary>
        ///     Creates entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Create(ICollection<T> entities)
        {
            Repository.Insert(entities);
            Repository.SaveDbChanges();
        }
        public async Task CreateAsync(ICollection<T> entities)
        {
            Repository.Insert(entities);
            await Repository.SaveDbChangesAsync();
        }

        #endregion Create methods

        #region Update methods

        /// <summary>
        ///     Updates entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            Repository.Update(entity);
            Repository.SaveDbChanges();
        }
        public async Task UpdateAsync(T entity)
        {
            Repository.Update(entity);
            await Repository.SaveDbChangesAsync();
        }

        /// <summary>
        ///     Updates entities
        /// </summary>
        /// <param name="entities"></param>
        public virtual void Update(ICollection<T> entities)
        {
            Repository.Update(entities);
            Repository.SaveDbChanges();
        }
        public async Task UpdateAsync(ICollection<T> entities)
        {
            Repository.Update(entities);
            await Repository.SaveDbChangesAsync();
        }

        #endregion Update methods

        #region Delete methods

        /// <summary>
        ///     Delete
        /// </summary>
        /// <param name="where"></param>
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            Repository.Delete(where);
            Repository.SaveDbChanges();
        }
        public async Task DeleteAsync(Expression<Func<T, bool>> where)
        {
            Repository.Delete(where);
            await Repository.SaveDbChangesAsync();
        }

        #endregion Delete methods
    }
}
