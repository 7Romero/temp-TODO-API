namespace ToDo.API.Dto.Task;

public class TaskDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsCompleted { get; set; }
    public Guid CategoryId { get; set; }

    public static TaskDto FromTask(Domain.Entity.Task task)
    {
        return new TaskDto
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            IsCompleted = task.IsCompleted,
            CategoryId = task.CategoryId
        };
    }
}
