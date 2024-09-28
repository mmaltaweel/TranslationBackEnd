using System.Net;
using System.Security.Claims;
using API.RequestDTO;
using Core.Interfaces;
using Ardalis.GuardClauses;
using Core.DTO.ResponseDTO;
using Core.DTO.Shared;
using Core.Enities.ProjectAggregate;
using Core.Exceptions;
using Core.Helpers;
using Core.Mapper;
using Core.Specifications;

namespace Core.Services;

public class ProjectService : IProjectService
{
    private readonly IAsyncRepository<Project> _projectRepository;
    private readonly IAsyncRepository<User> _userRepository;
    private ILog log;

    public ProjectService(IAsyncRepository<Project> projectRepository, IAsyncRepository<User> userRepository, ILog log)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        this.log = log;
    } 
    public async Task<ServiceResult<ProjectResponse>> CreateProject(ProjectRequest requestDto, ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project manager
            var projectManager = await _userRepository.GetByIdAsync(user.GetUserId());
            Guard.Against.Null(projectManager, nameof(projectManager));

            // Create the project entity
            var project = new Project(requestDto.Name, requestDto.Description, requestDto.StartDate, requestDto.EndDate,
                projectManager);

            await _projectRepository.AddAsync(project);
            return new ServiceResult<ProjectResponse>(project.ToProjectResponse(), true, "Project created",
                HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> UpdateProject(int projectId, ProjectRequest requestDto,
        ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");
            Guard.Against.Null(requestDto, nameof(requestDto), "Request Dto can not be null");

            var projectManager = project.ProjectManager;
            Guard.Against.Null(projectManager, nameof(projectManager), "Project manager not found");

            project.UpdateProject(requestDto.StartDate, requestDto.EndDate, requestDto.Name, requestDto.Description);

            // Save the changes
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(project.ToProjectResponse(), true, "Project Updated",
                HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<bool>> MarkProjectAsCompleted(int projectId, ClaimsPrincipal user)
    {
        try
        {
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action or project not exists");

            project.MarkProjectAsCompleted();
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<bool>(true, "Project Completed", HttpStatusCode.OK);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<bool>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<bool>(false, ex.Message, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> RemoveProject(int projectId, ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            // Remove the project (and its associated tasks)
            await _projectRepository.DeleteAsync(project);
            return new ServiceResult<ProjectResponse>(project.ToProjectResponse(), true, "Project Removed",
                HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> GetProjectsAssignedToManager(SharedParamFilter input)
    {
        try
        { 
            // sepc to load the project and tasks pagination 
            var spec = new ProjectWithTasksForManagerSpecification(input);
            // Fetch projects and their associated tasks for this manager
            var projects = await _projectRepository.ListAsync(spec);
            var result = projects.list.Select(p => p.ToProjectResponse()).ToList();

            return new ServiceResult<ProjectResponse>(result, true, "Projects Retrieved", HttpStatusCode.OK, projects.totalCount);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.InternalServerError);
        }
    }
}