using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDo.API.Dto.Pagination;
using ToDo.API.Dto.Task;
using ToDo.Bll.Interfaces;

namespace ToDo.API.Controllers;

[Route("api/task")]
public class TaskController : AppBaseController
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTask()
    {
        var tasks = await _taskService.GetTasks();

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskById(Guid id)
    {
        var task = await _taskService.GetTask(id);

        if (task == null) return NotFound();

        return Ok(task);
    }

    [Authorize]
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

        var category = await _taskService.CreateTask(task);

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, TaskDto.FromTask(task));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, TaskDto taskDto)
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

        var result = await _taskService.UpdateTask(id, task);

        if (!result)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var result = await _taskService.DeleteTask(id);

        if (!result)
        {
            return BadRequest();
        }

        return NoContent();
    }

    [HttpGet("pagination")]
    public async Task<IActionResult> GetPaginateTask([FromQuery] PaginationDto paginationDto)
    {
        var tasks = await _taskService.GetPaginateTasks(paginationDto.PageNumber, paginationDto.PageSize);

        return Ok(tasks);
    }
}
