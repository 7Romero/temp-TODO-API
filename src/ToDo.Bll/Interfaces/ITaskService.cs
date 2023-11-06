namespace ToDo.Bll.Interfaces;

public interface ITaskService
{
    Task<Domain.Entity.Task> GetTask(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entity.Task>> GetTasks(CancellationToken cancellationToken = default);
    Task<IEnumerable<Domain.Entity.Task>> GetPaginateTasks(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<Domain.Entity.Task> CreateTask(Domain.Entity.Task task, CancellationToken cancellationToken = default);
    Task<bool> UpdateTask(Guid id, Domain.Entity.Task newTask, CancellationToken cancellationToken = default);
    Task<bool> DeleteTask(Guid id, CancellationToken cancellationToken = default);
}
