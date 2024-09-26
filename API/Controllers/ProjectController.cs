using API.RequestDTO;
using Core.Enities.ProjectAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "ProjectManager")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _projectService.GetProjectsAssignedToManager(User);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] ProjectRequest requestDto)
    {
        var result = await _projectService.CreateProject(requestDto, User);
        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPut("{projectId}")]
    public async Task<IActionResult> Put(int projectId, [FromBody] ProjectRequest requestDto)
    {
        var result = await _projectService.UpdateProject(projectId, requestDto, User);
        if (result.Success)
        {
            return NoContent();
        }

        return BadRequest(result);
    }

    [HttpDelete("{projectId}")]
    public async Task<IActionResult> Delete(int projectId)
    {
        var result = await _projectService.RemoveProject(projectId, User);
        if (result.Success)
        {
            return NoContent();
        }

        return BadRequest(result);
    } 

    [HttpPut("{id}/status/completed")]
    public async Task<IActionResult> MarkProjectAsCompleted(int id)
    {
        var result=await _projectService.MarkProjectAsCompleted(id,User);
        if (result.Success)
        {
            return NoContent();
        }
        return BadRequest(result);
    } 
}