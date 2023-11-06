using Moq;
using ToDo.Bll.Services;
using ToDo.Dal.Interfaces;
using ToDo.Domain.Entity;

namespace ToDo.Bll.Tests.Services;

[TestClass]
public class TaskServiceTests
{
    private TaskService _taskService;
    private Mock<IGenericRepository> _genericRepositoryMock;

    [TestInitialize]
    public void Initialize()
    {
        _genericRepositoryMock = new Mock<IGenericRepository>();
        _taskService = new TaskService(_genericRepositoryMock.Object);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task CreateTask_ValidTask_ReturnsCreatedTask()
    {
        // Arrange
        var category = new Category { Id = Guid.NewGuid(), Name = "TestCategory" };
        var task = new Domain.Entity.Task { Title = "TestTask", Description = "TestDescription", CategoryId = category.Id };

        _genericRepositoryMock.Setup(x => x.GetById<Category>(category.Id)).ReturnsAsync(category);
        _genericRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        // Act
        var result = await _taskService.CreateTask(task);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(task, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async System.Threading.Tasks.Task DeleteTask_InvalidId_ThrowsArgumentNullException()
    {
        // Arrange
        var invalidId = Guid.Empty;
        _genericRepositoryMock.Setup(x => x.GetById<Domain.Entity.Task>(invalidId)).Returns(System.Threading.Tasks.Task.FromResult<Domain.Entity.Task>(null));

        // Act
        await _taskService.DeleteTask(invalidId);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task GetPaginateTasks_ValidRequest_ReturnsPaginatedListOfTasks()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var tasksList = new List<Domain.Entity.Task>
            {
                new Domain.Entity.Task { Title = "Task1" },
                new Domain.Entity.Task { Title = "Task2" },
                new Domain.Entity.Task { Title = "Task3" }
            };

        _genericRepositoryMock.Setup(x => x.PaginateWithInclude<Domain.Entity.Task>(pageNumber, pageSize))
            .ReturnsAsync(tasksList);

        // Act
        var result = await _taskService.GetPaginateTasks(pageNumber, pageSize);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(tasksList.Count, result.Count());
    }

    [TestMethod]
    public async System.Threading.Tasks.Task GetTask_ValidId_ReturnsTask()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var task = new Domain.Entity.Task { Id = taskId, Title = "TestTask" };

        _genericRepositoryMock.Setup(x => x.GetById<Domain.Entity.Task>(taskId))
            .ReturnsAsync(task);

        // Act
        var result = await _taskService.GetTask(taskId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(task, result);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task GetTasks_ValidRequest_ReturnsListOfTasks()
    {
        // Arrange
        var tasksList = new List<Domain.Entity.Task>
            {
                new Domain.Entity.Task { Title = "Task1" },
                new Domain.Entity.Task { Title = "Task2" },
                new Domain.Entity.Task { Title = "Task3" }
            };

        _genericRepositoryMock.Setup(x => x.GetAllWithInclude<Domain.Entity.Task>())
            .ReturnsAsync(tasksList);

        // Act
        var result = await _taskService.GetTasks();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(tasksList.Count, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async System.Threading.Tasks.Task UpdateTask_InvalidId_ThrowsArgumentNullException()
    {
        // Arrange
        var invalidId = Guid.Empty;
        var newTask = new Domain.Entity.Task { Title = "UpdatedTask" };
        _genericRepositoryMock.Setup(x => x.GetById<Domain.Entity.Task>(invalidId)).Returns(System.Threading.Tasks.Task.FromResult<Domain.Entity.Task>(null));

        // Act
        await _taskService.UpdateTask(invalidId, newTask);
    }

}
