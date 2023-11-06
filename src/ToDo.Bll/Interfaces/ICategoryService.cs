using ToDo.Domain.Entity;

namespace ToDo.Bll.Interfaces;

public interface ICategoryService
{
    Task<Category> GetCategory(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetPaginateCategories(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Category> CreateCategory(Category category, CancellationToken cancellationToken = default);
    Task<bool> UpdateCategory(Guid id, Category newCategory, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategory(Guid id, CancellationToken cancellationToken = default);
}
