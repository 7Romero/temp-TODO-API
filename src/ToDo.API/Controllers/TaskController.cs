using Microsoft.AspNetCore.Mvc;
using ToDo.API.Data.Interfaces;
using ToDo.API.Domain.Entity;
using ToDo.API.Dto.Pagination;
using ToDo.API.Dto.Task;

namespace ToDo.API.Controllers;

[Route("api/task")]
public class TaskController : AppBaseController
{
    private readonly IGenericRepository _genericRepository;

    public TaskController(IGenericRepository genericRepository)
    {
        _genericRepository = genericRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetTask()
    {
        var tasks = await _genericRepository.GetAllWithInclude<Domain.Entity.Task>();

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var task = await _genericRepository.GetById<Domain.Entity.Task>(id);

        if (task == null) return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(TaskDto taskDto)
    {
        if (taskDto.Title.Length < 4 || taskDto.Title.Length > 20)
        {
            return BadRequest("Title must be between 4 and 20 characters");
        }

        var task = new Domain.Entity.Task
        {
            Title = taskDto.Title,
            Description = taskDto.Description,
            CategoryId = taskDto.CategoryId,
            DueDate = taskDto.DueDate,
            IsCompleted = taskDto.IsCompleted
        };

        var category = await _genericRepository.GetById<Category>(taskDto.CategoryId);

        if (category == null) return NotFound($"Not found {nameof(category)} with Id: {taskDto.CategoryId}");

        _genericRepository.Add(task);

        await _genericRepository.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, TaskDto.FromTask(task));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, TaskDto taskDto)
    {
        if (taskDto.Title.Length < 4 || taskDto.Title.Length > 20)
        {
            return BadRequest("Title must be between 4 and 20 characters");
        }

        var task = await _genericRepository.GetById<Domain.Entity.Task>(id);

        if (task == null) return NotFound($"Not found {nameof(task)} with Id: {id}");

        var category = await _genericRepository.GetById<Category>(taskDto.CategoryId);

        if (category == null) return NotFound($"Not found {nameof(category)} with Id: {taskDto.CategoryId}");

        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.CategoryId = taskDto.CategoryId;
        task.DueDate = taskDto.DueDate;
        task.CreatedAt = taskDto.CreatedAt;
        task.IsCompleted = taskDto.IsCompleted;

        await _genericRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var task = await _genericRepository.GetById<Domain.Entity.Task>(id);

        if (task == null) return NotFound($"Not found {nameof(task)} with Id: {id}");

        await _genericRepository.Delete<Domain.Entity.Task>(id);

        await _genericRepository.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPaginateTask([FromQuery] PaginationDto paginationDto)
    {
        var tasks = await _genericRepository.PaginateWithInclude<Domain.Entity.Task>(paginationDto.PageNumber, paginationDto.PageSize);

        return Ok(tasks);
    }
}
