using Moq;
using ToDo.Bll.Services;
using ToDo.Dal.Interfaces;
using ToDo.Domain.Entity;

namespace ToDo.Bll.Tests.Services;

[TestClass]
public class CategoryServiceTests
{
    private CategoryService _categoryService;
    private Mock<IGenericRepository> _genericRepositoryMock;

    [TestInitialize]
    public void Initialize()
    {
        _genericRepositoryMock = new Mock<IGenericRepository>();
        _categoryService = new CategoryService(_genericRepositoryMock.Object);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task CreateCategory_ValidCategory_ReturnsCreatedCategory()
    {
        // Arrange
        var category = new Category { Name = "TestCategory" };

        _genericRepositoryMock.Setup(x => x.SaveChangesAsync()).Returns(System.Threading.Tasks.Task.CompletedTask);

        // Act
        var result = await _categoryService.CreateCategory(category);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(category, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async System.Threading.Tasks.Task DeleteCategory_InvalidId_ThrowsArgumentNullException()
    {
        // Arrange
        var invalidId = Guid.Empty;
        _genericRepositoryMock.Setup(x => x.GetById<Category>(invalidId)).Returns(System.Threading.Tasks.Task.FromResult<Category>(null));

        // Act
        await _categoryService.DeleteCategory(invalidId);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task GetCategories_ValidRequest_ReturnsListOfCategories()
    {
        // Arrange
        var categoriesList = new List<Category>
            {
                new Category { Name = "Category1" },
                new Category { Name = "Category2" },
                new Category { Name = "Category3" }
            };

        _genericRepositoryMock.Setup(x => x.GetAllWithInclude<Category>(c => c.Tasks))
            .ReturnsAsync(categoriesList);

        // Act
        var result = await _categoryService.GetCategories();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(categoriesList.Count, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async System.Threading.Tasks.Task GetCategory_InvalidId_ThrowsArgumentNullException()
    {
        // Arrange
        var invalidId = Guid.Empty;
        _genericRepositoryMock.Setup(x => x.GetByIdWithInclude<Category>(invalidId))
            .Returns(System.Threading.Tasks.Task.FromResult<Category>(null));

        // Act
        await _categoryService.GetCategory(invalidId);
    }

    [TestMethod]
    public async System.Threading.Tasks.Task GetPaginateCategories_ValidRequest_ReturnsPaginatedListOfCategories()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;
        var categoriesList = new List<Category>
            {
                new Category { Name = "Category1" },
                new Category { Name = "Category2" },
                new Category { Name = "Category3" }
            };

        _genericRepositoryMock.Setup(x => x.PaginateWithInclude<Category>(pageNumber, pageSize, c => c.Tasks))
            .ReturnsAsync(categoriesList);

        // Act
        var result = await _categoryService.GetPaginateCategories(pageNumber, pageSize);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(categoriesList.Count, result.Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public async System.Threading.Tasks.Task UpdateCategory_InvalidId_ThrowsArgumentNullException()
    {
        // Arrange
        var invalidId = Guid.Empty;
        var newCategory = new Category { Name = "UpdatedCategory" };
        _genericRepositoryMock.Setup(x => x.GetById<Category>(invalidId)).Returns(System.Threading.Tasks.Task.FromResult<Category>(null));

        // Act
        await _categoryService.UpdateCategory(invalidId, newCategory);
    }
}
