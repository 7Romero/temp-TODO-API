using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDo.API.Controllers;
using ToDo.API.Dto.Category;
using ToDo.API.Dto.Pagination;
using ToDo.Bll.Interfaces;

namespace ToDo.Tests;

[TestFixture]
public class CategoryControllerTests
{
    [Test]
    public async Task GetCategory_ReturnsOkResult()
    {
        // Arrange
        var mockCategoryService = new Mock<ICategoryService>();
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.GetCategory();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task GetCategoryById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var mockCategoryService = new Mock<ICategoryService>();
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.GetCategoryById(categoryId);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task CreateCategory_WithValidInput_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var categoryDto = new CategoryDto
        {
            Name = "Category 1"
        };
        var mockCategoryService = new Mock<ICategoryService>();
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.CreateCategory(categoryDto);

        // Assert
        Assert.IsInstanceOf<CreatedAtActionResult>(result);
    }

    [Test]
    public async Task UpdateCategory_WithValidInput_ReturnsNoContentResult()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var categoryDto = new CategoryDto
        {
            Name = "Category 1"
        };
        var mockCategoryService = new Mock<ICategoryService>();
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.UpdateCategory(categoryId, categoryDto);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task DeleteCategory_WithValidId_ReturnsNoContentResult()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var mockCategoryService = new Mock<ICategoryService>();
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.DeleteCategory(categoryId);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task GetCategoryWithPagination_ReturnsOkResult()
    {
        // Arrange
        var paginationDto = new PaginationDto
        {
            PageNumber = 1,
            PageSize = 10
        };
        var mockCategoryService = new Mock<ICategoryService>();
        var controller = new CategoryController(mockCategoryService.Object);

        // Act
        var result = await controller.GetCategoryWithPagination(paginationDto);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }
}
