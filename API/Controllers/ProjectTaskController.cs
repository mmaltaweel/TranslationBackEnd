using API.Controllers.Helpers;
using API.RequestDTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectTaskController : Controller
{
    private readonly IProjectTaskService _projectTaskService;
    public ProjectTaskController(IProjectService projectService, IProjectTaskService projectTaskService)
    {
        _projectTaskService = projectTaskService;
    }

    [HttpGet]
    [Authorize(Roles = "Translator, ProjectManager")]
    public async Task<IActionResult> Get([FromQuery] int skip = 0, [FromQuery] int take = 7)
    {
        var filter = ControllersHelper.CreateSharedParamFilter(skip, take, User);
        var result = await _projectTaskService.GetTasksAssignedToTranslator(filter);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("{projectId}/tasks")]
    [Authorize(Roles = "ProjectManager")]
    public async Task<IActionResult> Post(int projectId, [FromBody] ProjectTaskRequest requestDto)
    {
        var result = await _projectTaskService.CreateTask(projectId, requestDto, User);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("{projectId}/tasks/{taskId}")]
    [Authorize(Roles = "ProjectManager")]
    public async Task<IActionResult> Put(int projectId, int taskId, [FromBody] ProjectTaskRequest requestDto)
    {
        var result = await _projectTaskService.UpdateTask(projectId, requestDto, taskId, User);
        if (result.Success)
        {
            return NoContent();
        }

        return BadRequest(result);
    }

    [HttpDelete("{projectId}/tasks/{taskId}")]
    [Authorize(Roles = "ProjectManager")]
    public async Task<IActionResult> Delete(int projectId, int taskId)
    {
        var result = await _projectTaskService.RemoveTask(projectId, taskId, User);
        if (result.Success)
        {
            return NoContent();
        }

        return BadRequest(result);
    }

    [HttpPut("{taskId}/status/completed")]
    [Authorize(Roles = "Translator")]
    public async Task<IActionResult> MarkTaskAsCompleted(int taskId)
    {
        var result = await _projectTaskService.MarkTaskAsCompleted(taskId, User);
        if (result.Success)
        {
            return NoContent();
        }

        return BadRequest(result);
    }
}