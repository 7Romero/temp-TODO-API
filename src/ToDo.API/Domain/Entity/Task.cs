using ToDo.API.Domain.Entity.Interfaces;

namespace ToDo.API.Domain.Entity;

public class Task : IBaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    public Guid CategoryId { get; set; }
}
