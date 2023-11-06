using ToDo.Domain.Entity.Interfaces;

namespace ToDo.Domain.Entity;

public class Category : IBaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IList<Task> Tasks { get; set; }
}
