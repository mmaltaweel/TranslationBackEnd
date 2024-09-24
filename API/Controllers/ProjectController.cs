using API.RequestDTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize(Roles = "ProjectManager")]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    [HttpGet]
    public async Task<IActionResult> GetProjectsForManager()
    { 
        var result = await _projectService.GetProjectsByManager(User);

        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] ProjectRequest requestDto)
    {
        var result= await _projectService.CreateProject(requestDto,User);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPatch("{projectId}")]
    public async Task<IActionResult> PatchProject(int projectId, [FromBody] ProjectRequest requestDto)
    {
        var result= await _projectService.UpdateProject(projectId, requestDto,User);
        if (result.Success)
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> RemoveProject(int projectId)
    {
       var result=await _projectService.RemoveProject(projectId,User);
       if (result.Success)
       {
           return NoContent();
       }
       return BadRequest(result);
    }

    [HttpPost("{projectId}/tasks")]
    public async Task<IActionResult> CreateTask(int projectId, [FromBody] ProjectTaskRequest requestDto)
    {
        var result=await _projectService.CreateTask(projectId,requestDto,User);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }

    [HttpPatch("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> PatchTask(int projectId, int taskId, [FromBody] ProjectTaskRequest requestDto)
    {
        var result=await _projectService.UpdateTask(projectId, requestDto,User);
        if (result.Success)
        {
            return NoContent();
        }
        return BadRequest(result);
    }

    [HttpDelete("{projectId}/tasks/{taskId}")]
    public async Task<IActionResult> RemoveTask(int projectId, int taskId)
    {
        var result=await _projectService.RemoveTask(projectId, taskId,User);
        if (result.Success)
        {
            return NoContent();
        }
        return BadRequest(result);
    }
}