using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDo.Domain.Entity;
using ToDo.Domain.Entity.Auth;

namespace ToDo.Dal;

public class AppDbContext : IdentityDbContext<User, Role, Guid>
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

        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<Role>().ToTable("Roles");
    }
}
