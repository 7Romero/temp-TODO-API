using ToDo.Bll.Interfaces;
using ToDo.Dal.Interfaces;
using ToDo.Domain.Entity;

namespace ToDo.Bll.Services;

public class CategoryService : ICategoryService
{
    private readonly IGenericRepository _genericRepository;

    public CategoryService(IGenericRepository genericRepository) 
    {
        _genericRepository = genericRepository;
    }

    public async Task<Category> CreateCategory(Category category, CancellationToken cancellationToken = default)
    {
        _genericRepository.Add(category);

        await _genericRepository.SaveChangesAsync();

        return category;
    }

    public async Task<bool> DeleteCategory(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _genericRepository.GetById<Category>(id);

        if (category == null) throw new ArgumentNullException($"Not found {nameof(category)} with Id: {id}");

        await _genericRepository.Delete<Category>(id);

        await _genericRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Category>> GetCategories(CancellationToken cancellationToken = default)
    {
        var categories = await _genericRepository.GetAllWithInclude<Category>(c => c.Tasks);

        return categories;
    }

    public async Task<Category> GetCategory(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _genericRepository.GetByIdWithInclude<Category>(id, c => c.Tasks);

        if (category == null) throw new ArgumentNullException($"Not found {nameof(category)} with Id: {id}");

        return category;
    }

    public async Task<IEnumerable<Category>> GetPaginateCategories(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var categories = await _genericRepository.PaginateWithInclude<Category>(pageNumber, pageSize, c => c.Tasks);

        return categories;
    }

    public async Task<bool> UpdateCategory(Guid id, Category newCategory, CancellationToken cancellationToken = default)
    {
        var category = await _genericRepository.GetById<Category>(id);

        if (category == null) throw new ArgumentNullException($"Not found {nameof(category)} with Id: {id}");

        category.Name = newCategory.Name;

        await _genericRepository.SaveChangesAsync();

        return true;
    }
}
