using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Controllers;
using ToDo.API.Dto.Pagination;
using ToDo.API.Dto.Task;
using ToDo.Bll.Services;
using ToDo.Dal;
using ToDo.Dal.Repository;

namespace ToDo.Tests;

[TestFixture]
public class TaskControllerTests
{
    private DbContextOptions<AppDbContext> _options;
    private AppDbContext _dbContext;

    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

        _dbContext = new AppDbContext(_options);
    }

    [Test]
    public async Task GetTask_ReturnsOkResult()
    {
        // Arrange
        var task = new Domain.Entity.Task
        {
            Title = "Task 1",
            Description = "Description 1"
        };

        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var repository = new GenericRepository(_dbContext);
        var service = new TaskService(repository);
        var controller = new TaskController(service);

        // Act
        var result = await controller.GetTask();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task GetTaskById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        var task = new Domain.Entity.Task
        {
            Title = "Task 1",
            Description = "Description 1"
        };

        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var repository = new GenericRepository(_dbContext);
        var service = new TaskService(repository);
        var controller = new TaskController(service);

        // Act
        var result = await controller.GetTaskById(task.Id);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public async Task CreateTask_WithValidInput_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var category = new Domain.Entity.Category
        {
            Name = "Category 1"
        };

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        var taskDto = new TaskDto
        {
            Title = "Task 1",
            Description = "Description 1",
            CategoryId = category.Id
        };

        var repository = new GenericRepository(_dbContext);
        var service = new TaskService(repository);
        var controller = new TaskController(service);

        // Act
        var result = await controller.CreateTask(taskDto);

        // Assert
        Assert.IsInstanceOf<CreatedAtActionResult>(result);
    }

    [Test]
    public async Task UpdateTask_WithValidInput_ReturnsNoContentResult()
    {
        // Arrange
        var category = new Domain.Entity.Category
        {
            Name = "Category 1"
        };

        var task = new Domain.Entity.Task
        {
            Title = "Task 1",
            Description = "Description 1",
            CategoryId = category.Id
        };

        _dbContext.Categories.Add(category);
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var taskDto = new TaskDto
        {
            Title = "Task 2",
            Description = "Description 2",
            CategoryId = category.Id
        };
        var repository = new GenericRepository(_dbContext);
        var service = new TaskService(repository);
        var controller = new TaskController(service);

        // Act
        var result = await controller.UpdateTask(task.Id, taskDto);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task DeleteTask_WithValidId_ReturnsNoContentResult()
    {
        // Arrange
        var task = new Domain.Entity.Task
        {
            Title = "Task 1",
            Description = "Description 1"
        };

        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        var repository = new GenericRepository(_dbContext);
        var service = new TaskService(repository);
        var controller = new TaskController(service);

        // Act
        var result = await controller.DeleteTask(task.Id);

        // Assert
        Assert.IsInstanceOf<NoContentResult>(result);
    }

    [Test]
    public async Task GetPaginateTask_ReturnsOkResult()
    {
        // Arrange
        var paginationDto = new PaginationDto
        {
            PageNumber = 1,
            PageSize = 10
        };

        var repository = new GenericRepository(_dbContext);
        var service = new TaskService(repository);
        var controller = new TaskController(service);

        // Act
        var result = await controller.GetPaginateTask(paginationDto);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
    }


    [TearDown]
    public void TearDown()
    {
        _dbContext.Dispose();
    }
}