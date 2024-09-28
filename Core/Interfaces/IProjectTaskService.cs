using System.Security.Claims;
using API.RequestDTO;
using Core.DTO.ResponseDTO;

namespace Core.Interfaces;

public interface IProjectTaskService
{
    Task<ServiceResult<ProjectResponse>> CreateTask(int projectId, ProjectTaskRequest requestDto, ClaimsPrincipal user);
    Task<ServiceResult<ProjectResponse>> UpdateTask(int projectId, ProjectTaskRequest requestDto, int taskId, ClaimsPrincipal user);
    Task<ServiceResult<bool>> MarkTaskAsCompleted(int taskId, ClaimsPrincipal user);
    Task<ServiceResult<TaskResponse>> GetTasksAssignedToTranslator(ClaimsPrincipal translatorUser);
    Task<ServiceResult<ProjectResponse>> RemoveTask(int projectId, int taskId, ClaimsPrincipal user);
}