using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using SkillApp.Entities.Base;

namespace SkillApp.Data.Interfaces.Base
{
    public interface IReadOnlyRepository<T> where T : BaseEntity
    {
        /// <summary>
        ///     Get all entities
        /// </summary>
        /// <param name="includes">Additional params</param>
        /// <returns>List of T entities</returns>
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

        /// <summary>
        ///     Get list of entities by given expression where criteria
        /// </summary>
        /// <param name="where">criteria in form of LINQ: x => *expression*</param>
        /// <param name="includes">Additional params</param>
        /// <returns>List of T entities</returns>
        IQueryable<T> GetBy(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);

        /// <summary>
        ///     Count all entities
        /// </summary>
        /// <returns>Number of all entities</returns>
        int Count();

        /// <summary>
        ///     Count all entities for given expression
        /// </summary>
        /// <param name="where">Expression</param>
        /// <returns>Number of entities</returns>
        int Count(Expression<Func<T, bool>> where);

        /// <summary>
        ///     Check if entity exists for the given expression
        /// </summary>
        /// <param name="where">Expression</param>
        /// <returns>True or False</returns>
        bool Exists(Expression<Func<T, bool>> where);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        Task<int> CountAsync();
        Task<bool> ExistsAsync(Expression<Func<T, bool>> where);
    }
}
