using System.Security.Claims;
using API.RequestDTO;
using Core.DTO.ResponseDTO;
using Core.Enities.ProjectAggregate;

namespace Core.Interfaces;

public interface IProjectService
{
    Task<ServiceResult<ProjectResponse>> CreateProject(ProjectRequest requestDto, ClaimsPrincipal user);
    Task<ServiceResult<ProjectResponse>> UpdateProject(int projectId, ProjectRequest requestDto, ClaimsPrincipal user);
    Task<ServiceResult<bool>> MarkProjectAsCompleted(int projectId, ClaimsPrincipal user);
    Task<ServiceResult<ProjectResponse>> RemoveProject(int projectId, ClaimsPrincipal user);
    Task<ServiceResult<ProjectResponse>> GetProjectsAssignedToManager(ClaimsPrincipal managerUser);
}