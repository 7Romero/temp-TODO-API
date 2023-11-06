using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Dto.Category;
using ToDo.API.Dto.Pagination;
using ToDo.Bll.Interfaces;
using ToDo.Domain.Entity;

namespace ToDo.API.Controllers;

[Route("api/category")]
public class CategoryController : AppBaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategory()
    {
        var categories = await _categoryService.GetCategories();

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var category = await _categoryService.GetCategory(id);

        return Ok(category);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name
        };

        await _categoryService.CreateCategory(category);

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, CategoryDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name
        };

        await _categoryService.UpdateCategory(id, category);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await _categoryService.DeleteCategory(id);

        return NoContent();
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetCategoryWithPagination([FromQuery] PaginationDto paginationDto)
    {
        var categories = await _categoryService.GetPaginateCategories(paginationDto.PageNumber, paginationDto.PageSize);

        return Ok(categories);
    }
}
