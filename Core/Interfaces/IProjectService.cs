using System.Security.Claims;
using API.RequestDTO;
using Core.DTO.ResponseDTO;
using Core.Enities.ProjectAggregate;

namespace Core.Interfaces;

public interface IProjectService
{
    Task<ServiceResult<ProjectResponse>> CreateProject(ProjectRequest requestDto, ClaimsPrincipal user);

    Task<ServiceResult<ProjectResponse>> UpdateProject(int projectId, ProjectRequest requestDto, ClaimsPrincipal user);

    Task<ServiceResult<ProjectResponse>> RemoveProject(int projectId, ClaimsPrincipal user);

    Task<ServiceResult<ProjectResponse>> StartProject(int projectId, ClaimsPrincipal user);

    Task<ServiceResult<ProjectResponse>> CompleteProject(int projectId, ClaimsPrincipal user);
    Task<ServiceResult<ProjectResponse>> GetProjectsByManager(ClaimsPrincipal managerUser);

    Task<ServiceResult<ProjectResponse>> CreateTask(int projectId, ProjectTaskRequest requestDto, ClaimsPrincipal user);

    Task<ServiceResult<ProjectResponse>> UpdateTask(int projectId, ProjectTaskRequest requestDto, ClaimsPrincipal user);

    Task<ServiceResult<ProjectResponse>> RemoveTask(int projectId, int taskId, ClaimsPrincipal user);

    Task<ServiceResult<ProjectResponse>> StartTask(int projectId,int taskId, ClaimsPrincipal user);

    Task<ServiceResult<ProjectResponse>> CompleteTask(int projectId,int taskId, ClaimsPrincipal user);
    
   
}