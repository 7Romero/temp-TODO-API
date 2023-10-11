using Microsoft.AspNetCore.Mvc;
using ToDo.API.Data.Interfaces;
using ToDo.API.Domain.Entity;
using ToDo.API.Dto.Category;
using ToDo.API.Dto.Pagination;

namespace ToDo.API.Controllers;

[Route("api/category")]
public class CategoryController : AppBaseController
{
    private readonly IGenericRepository _genericRepository;

    public CategoryController(IGenericRepository genericRepository)
    {
        _genericRepository = genericRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategory()
    {
        var categories = await _genericRepository.GetAllWithInclude<Category>(c => c.Tasks);

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var category = await _genericRepository.GetByIdWithInclude<Category>(id, c => c.Tasks);

        if (category == null) return NotFound();

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
    {
        var category = new Category 
        {
            Name = categoryDto.Name 
        };

        _genericRepository.Add(category);
        await _genericRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, CategoryDto categoryDto)
    {
        var category = await _genericRepository.GetById<Category>(id);

        if (category == null) return NotFound();

        category.Name = categoryDto.Name;

        await _genericRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await _genericRepository.GetById<Category>(id);

        if (category == null) return NotFound();

        await _genericRepository.Delete<Category>(id);

        await _genericRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetCategoryWithPagination([FromQuery] PaginationDto paginationDto)
    {
        var categories = await _genericRepository.PaginateWithInclude<Category>(paginationDto.PageNumber, paginationDto.PageSize, c => c.Tasks);

        return Ok(categories);
    }
}
