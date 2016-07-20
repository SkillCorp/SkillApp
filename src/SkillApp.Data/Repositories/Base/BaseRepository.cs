using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using SkillApp.Data.Interfaces.Base;
using SkillApp.Entities.Base;

namespace SkillApp.Data.Repositories.Base
{
    /// <summary>
    ///     Represents the base repository class.
    ///     A repository is responsible for encapsulating the data access code.
    ///     It sits between the DAL and the business layer of the application to query the data source for data and map this
    ///     data to an entity class,
    ///     and it also persists changes in the entity classes back to the data source using the context.
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public abstract class BaseRepository<T> : ReadOnlyBaseRepository<T>, IRepository<T> where T : BaseEntity
    {
        public BaseRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }

        public void Detach(T entity)
        {
            MyDbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            foreach (var entity in Get().Where(where))
            {
                MyDbContext.Entry(entity).State = EntityState.Deleted;
            }
        }
        public void Delete(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
        public void Delete(T entity)
        {
            MyDbContext.Entry(entity).State = EntityState.Deleted;
        }

        public void Insert(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }
        public void Insert(T entity)
        {
            MyDbContext.Set<T>().Add(entity);
        }

        public void Update(Expression<Func<T, bool>> where)
        {
            foreach (var entity in Get().Where(where))
            {
                MyDbContext.Entry(entity).State = EntityState.Modified;
            }
        }
        public void Update(ICollection<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }
        public void Update(T entity)
        {
            MyDbContext.Entry(entity).State = EntityState.Modified;
        }

        public void SaveDbChanges()
        {
            try
            {
                MyDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                //TODO: Do better error handling here
                throw;
            }
        }
        public async Task SaveDbChangesAsync()
        {
            try
            {
                await MyDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //TODO: Do better error handling here
                throw;
            }
        }
    }
}
