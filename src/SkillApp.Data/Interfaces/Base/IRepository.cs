using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using SkillApp.Entities.Base;

namespace SkillApp.Data.Interfaces.Base
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : BaseEntity
    {
        /// <summary>
        ///     Detach entity
        /// </summary>
        /// <param name="entity">Generic Entity</param>
        void Detach(T entity);

        /// <summary>
        ///     Inserts entity in database
        /// </summary>
        /// <param name="entity">Generic Entity</param>
        void Insert(T entity);
        /// <summary>
        ///     Inserts collection of entities in database
        /// </summary>
        /// <param name="entities">Collection of entities</param>
        void Insert(ICollection<T> entities);

        /// <summary>
        ///     Updates entity in database
        /// </summary>
        /// <param name="entity">Generic Entity</param>
        void Update(T entity);
        /// <summary>
        ///     Updates entity in database
        /// </summary>
        /// <param name="entities">Generic Entity</param>
        void Update(ICollection<T> entities);
        /// <summary>
        ///     Deletes entities from database
        /// </summary>
        /// <param name="where">Where clause</param>
        void Update(Expression<Func<T, bool>> where);

        /// <summary>
        ///     Deletes entity from database
        /// </summary>
        /// <param name="entity">Generic Entity</param>
        void Delete(T entity);
        /// <summary>
        ///     Deletes collection of entities from database
        /// </summary>
        /// <param name="entities">Collection of entities</param>
        void Delete(ICollection<T> entities);
        /// <summary>
        ///     Deletes entities from database
        /// </summary>
        /// <param name="where">Where clause</param>
        void Delete(Expression<Func<T, bool>> where);

        void SaveDbChanges();
        Task SaveDbChangesAsync();
    }
}
