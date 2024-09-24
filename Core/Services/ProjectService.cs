using System.Net;
using System.Security.Claims;
using API.RequestDTO;
using Core.Interfaces;
using Ardalis.GuardClauses;
using AutoMapper;
using Core.DTO.ResponseDTO;
using Core.Enities.ProjectAggregate;
using Core.Exceptions;
using Core.Helpers;
using Core.Specifications;

namespace Core.Services;

public class ProjectService : IProjectService
{
    private readonly IAsyncRepository<Project> _projectRepository;
    private readonly IAsyncRepository<User> _userRepository;
    private ILog log;
    private readonly IMapper _mapper;


    public ProjectService(IAsyncRepository<Project> projectRepository, IAsyncRepository<User> userRepository, ILog log,IMapper mapper)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        this.log = log;
        _mapper = mapper;
    }

    #region Project domain logic

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
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Project created", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
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

            project.UpdateManager(projectManager);
            project.UpdateStartDate(requestDto.StartDate);
            project.UpdateEndDate(requestDto.EndDate);

            // Save the changes
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Project Updated", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
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
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Project Removed", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> StartProject(int projectId, ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            project.StartProject();
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Project Started", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> CompleteProject(int projectId, ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            project.CompleteProject();
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Project Completed", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }
    public async Task<ServiceResult<ProjectResponse>> GetProjectsByManager(ClaimsPrincipal managerUser)
    {
        try
        {        // Ensure the user is a Project Manager
                 var user = await _userRepository.GetByIdAsync(managerUser.GetUserId());
                 
                 if (user == null || user.Role != UserRole.ProjectManager)
                 {
                     throw new UnauthorizedAccessException("User is not a manager");
                 }
         
                 // Fetch projects and their associated tasks for this manager
                 var projects = user.ManagedProjects;
                 
                 var result= projects.Select(p => new ProjectResponse
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Description = p.Description,
                     StartDate = p.StartDate,
                     EndDate = p.EndDate,
                     Tasks = p.Tasks.Select(t => new TaskResponse
                     {
                         Id = t.Id,
                         Title = t.Title,
                         Status = t.Status,
                         Description = t.Description,
                         DueDate = t.DueDate,
                          AssignedTranslatorId = t.AssignedTranslatorId
                     }).ToList()
                 }).ToList();
                 
                 return new ServiceResult<ProjectResponse>(result,true, "Projects Retrieved", HttpStatusCode.OK);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }

    }
    #endregion

    #region Task domain logic

    public async Task<ServiceResult<ProjectResponse>> CreateTask(int projectId, ProjectTaskRequest requestDto,
        ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            // Assign the task to the project
            //check if the AssignedTranslatorId is there 
            var taskAssignee = await _userRepository.GetByIdAsync(requestDto.AssignedTranslatorId);
            Guard.Against.Null(taskAssignee, nameof(taskAssignee));
            project.CreateTask(requestDto.Title, requestDto.Description, requestDto.DueDate, taskAssignee);

            // Update the project with the new task
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Task created", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> UpdateTask(int projectId, ProjectTaskRequest requestDto,
        ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            project.UpdateTask(requestDto.Id, requestDto.AssignedTranslatorId, requestDto.Title, requestDto.Description,
                requestDto.DueDate, user.GetUserId());

            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Task updated", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> RemoveTask(int projectId, int taskId, ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            // Find and remove the specific task
            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            Guard.Against.Null(task, nameof(task));

            project.RemoveTask(task);

            // Update the project in the repository
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Task Removed", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> StartTask(int projectId, int taskId, ClaimsPrincipal user)
    {
        try
        {
            var specification = new GetTranslatorsAuthorizedTasksSpecification(projectId, taskId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            // Find the specific task
            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            Guard.Against.Null(task, nameof(task));

            // Complete the task
            task.StartTask();

            // Update the project in the repository
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Task Started", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> CompleteTask(int projectId, int taskId, ClaimsPrincipal user)
    {
        try
        {
            var specification = new GetTranslatorsAuthorizedTasksSpecification(projectId, taskId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            // Find the specific task
            var task = project.Tasks.FirstOrDefault(t => t.Id == taskId);
            Guard.Against.Null(task, nameof(task));

            // Complete the task
            task.CompleteTask();

            // Update the project in the repository
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(_mapper.Map<ProjectResponse>(project), true, "Task Completed", HttpStatusCode.Created);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }
    #endregion
}