using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using ToDo.API.Data.Interfaces;
using ToDo.API.Domain.Entity.Interfaces;

namespace ToDo.API.Data.Repository;

public class GenericRepository : IGenericRepository
{
    private readonly AppDbContext _appDbContext;

    public GenericRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Add<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
    {
        _appDbContext.Set<TEntity>().Add(entity);
    }

    public async Task<TEntity> Delete<TEntity>(Guid id) where TEntity : class, IBaseEntity
    {
        var entity = await _appDbContext.Set<TEntity>().FindAsync(id);
        if (entity == null)
        {
            throw new ValidationException($"Object of type {typeof(TEntity)} with id {id} not found");
        }

        _appDbContext.Set<TEntity>().Remove(entity);

        return entity;
    }

    public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class, IBaseEntity
    {
        return await _appDbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<List<TEntity>> PaginateWithInclude<TEntity>(int pageNumber, int pageSize, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
    {
        var query = IncludeProperties(includeProperties);

        return await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<TEntity> GetById<TEntity>(Guid id) where TEntity : class, IBaseEntity
    {
        return await _appDbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TEntity> GetByIdWithInclude<TEntity>(Guid id, params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
    {
        var query = IncludeProperties(includeProperties);

        return await query.FirstOrDefaultAsync(entity => entity.Id == id);
    }
    public async Task<List<TEntity>> GetAllWithInclude<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
    {
        var query = IncludeProperties(includeProperties);

        return await query.ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<TEntity> GetFirstWithInclude<TEntity>(Expression<Func<TEntity, bool>> predicate,
                                                   params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
    {
        var query = IncludeProperties(includeProperties);

        var entities = await query.FirstOrDefaultAsync(predicate);

        if (entities == null)
        {
            throw new ValidationException($"Object of type {typeof(TEntity)} was not found");
        }

        return entities;
    }

    public async Task<List<TEntity>> GetByWhereWithInclude<TEntity>(Expression<Func<TEntity, bool>> predicate,
                                                       params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
    {
        var query = IncludeProperties(includeProperties);

        var entities = await query.Where(predicate).ToListAsync();

        if (entities == null)
        {
            throw new ValidationException($"Object of type {typeof(TEntity)} was not found");
        }

        return entities;
    }

    private IQueryable<TEntity> IncludeProperties<TEntity>(params Expression<Func<TEntity, object>>[] includeProperties) where TEntity : class, IBaseEntity
    {
        IQueryable<TEntity> entities = _appDbContext.Set<TEntity>();

        foreach (var includeProperty in includeProperties)
        {
            entities = entities.Include(includeProperty);
        }

        return entities;
    }
}
