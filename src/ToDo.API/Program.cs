using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Infrastructure.Extensions;
using ToDo.Bll.Interfaces;
using ToDo.Bll.Services;
using ToDo.Dal;
using ToDo.Dal.Interfaces;
using ToDo.Dal.Repository;
using ToDo.Domain.Entity.Auth;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// For Entity Framework
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
});

// Adding Authentication

builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequiredLength = 5;
})
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IGenericRepository, GenericRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Config auth

var authOptions = builder.Services.ConfigureAuthOptions(configuration);
builder.Services.AddJwtAuthentication(authOptions);
builder.Services.AddSwagger(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
