using Microsoft.EntityFrameworkCore;
using System.Data;
using ToDo.API.Domain.Entity;

namespace ToDo.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {

    }
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Domain.Entity.Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
