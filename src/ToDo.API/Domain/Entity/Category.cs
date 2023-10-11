using ToDo.API.Domain.Entity.Interfaces;

namespace ToDo.API.Domain.Entity;

public class Category : IBaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IList<Task> Tasks { get; set; }
}
