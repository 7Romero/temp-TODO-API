using ToDo.Dal.Interfaces;
using ToDo.Bll.Interfaces;
using ToDo.Domain.Entity;

namespace ToDo.Bll.Services;

public class TaskService : ITaskService
{
    private readonly IGenericRepository _genericRepository;

    public TaskService(IGenericRepository genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<Domain.Entity.Task> CreateTask(Domain.Entity.Task task, CancellationToken cancellationToken = default)
    {
        var category = await _genericRepository.GetById<Category>(task.CategoryId);

        if (category == null) throw new ArgumentNullException($"Not found {nameof(category)} with Id: {task.CategoryId}");

        _genericRepository.Add(task);

        await _genericRepository.SaveChangesAsync();

        return task;
    }

    public async Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await _genericRepository.GetById<Domain.Entity.Task>(id);

        if (task == null) throw new ArgumentNullException($"Not found {nameof(task)} with Id: {id}");

        await _genericRepository.Delete<Domain.Entity.Task>(id);

        await _genericRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Domain.Entity.Task>> GetPaginateTasks(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var tasks = await _genericRepository.PaginateWithInclude<Domain.Entity.Task>(pageNumber, pageSize);

        return tasks;
    }

    public async Task<Domain.Entity.Task> GetTask(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await _genericRepository.GetById<Domain.Entity.Task>(id);

        return task;
    }

    public async Task<IEnumerable<Domain.Entity.Task>> GetTasks(CancellationToken cancellationToken = default)
    {
        var tasks = await _genericRepository.GetAllWithInclude<Domain.Entity.Task>();

        return tasks;
    }

    public async Task<bool> UpdateTask(Guid id,Domain.Entity.Task newTask, CancellationToken cancellationToken = default)
    {
        var task = await _genericRepository.GetById<Domain.Entity.Task>(id);

        if (task == null) throw new ArgumentNullException($"Not found {nameof(task)} with Id: {id}");

        var category = await _genericRepository.GetById<Category>(newTask.CategoryId);

        if (category == null) throw new ArgumentNullException($"Not found {nameof(category)} with Id: {newTask.CategoryId}");

        task.Title = newTask.Title;
        task.Description = newTask.Description;
        task.CategoryId = newTask.CategoryId;
        task.DueDate = newTask.DueDate;
        task.CreatedAt = newTask.CreatedAt;
        task.IsCompleted = newTask.IsCompleted;

        await _genericRepository.SaveChangesAsync();

        return true;
    }
}
