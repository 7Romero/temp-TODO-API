using System.Linq.Expressions;
using ToDo.Domain.Entity.Interfaces;

namespace ToDo.Dal.Interfaces;

public interface IGenericRepository
{
    Task<TEntity> GetById<TEntity>(Guid id) where TEntity : class, IBaseEntity;

    Task<List<TEntity>> GetAllWithInclude<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity;

    Task<TEntity> GetFirstWithInclude<TEntity>(Expression<Func<TEntity, bool>> predicate,
                                                params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity;

    Task<List<TEntity>> GetByWhereWithInclude<TEntity>(Expression<Func<TEntity, bool>> predicate,
                                                params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity;

    Task<TEntity> GetByIdWithInclude<TEntity>(Guid id, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity;

    Task<List<TEntity>> GetAll<TEntity>() where TEntity : class, IBaseEntity;

    Task<List<TEntity>> PaginateWithInclude<TEntity>(int pageNumber, int pageSize, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity;

    Task SaveChangesAsync();

    void Add<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

    Task<TEntity> Delete<TEntity>(Guid id) where TEntity : class, IBaseEntity;
}
