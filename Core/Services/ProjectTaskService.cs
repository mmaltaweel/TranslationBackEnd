using System.Net;
using System.Security.Claims;
using API.RequestDTO;
using Ardalis.GuardClauses;
using Core.DTO.ResponseDTO;
using Core.DTO.Shared;
using Core.Enities.ProjectAggregate;
using Core.Exceptions;
using Core.Helpers;
using Core.Interfaces;
using Core.Mapper;
using Core.Specifications;

namespace Core.Services;

public class ProjectTaskService : IProjectTaskService
{
    private readonly IAsyncRepository<Project> _projectRepository;
    private readonly IAsyncRepository<ProjectTask> _projectTaskRepository;

    private readonly IAsyncRepository<User> _userRepository;
    private ILog log;

    public ProjectTaskService(IAsyncRepository<Project> projectRepository, IAsyncRepository<User> userRepository,
        ILog log, IAsyncRepository<ProjectTask> projectTaskRepository)
    {
        _projectRepository = projectRepository;
        _userRepository = userRepository;
        this.log = log;
        _projectTaskRepository = projectTaskRepository;
    }

    public async Task<ServiceResult<ProjectResponse>> CreateTask(int projectId, ProjectTaskRequest requestDto,
        ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");
            Guard.Against.Null(requestDto, nameof(requestDto), "requestDto is null");
            // Assign the task to the project
            //check if the AssignedTranslatorId is there 
            var taskAssignee = await _userRepository.GetByIdAsync(requestDto.AssignedTranslatorId);
            Guard.Against.Null(taskAssignee, nameof(taskAssignee));
            project.CreateTask(requestDto.Title, requestDto.Description, requestDto.DueDate, taskAssignee);

            // Update the project with the new task
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(project.ToProjectResponse(), true, "Task created",
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
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }

    public async Task<ServiceResult<ProjectResponse>> UpdateTask(int projectId, ProjectTaskRequest requestDto,
        int taskId
        , ClaimsPrincipal user)
    {
        try
        {
            // Fetch the project entity and check if the logged in manager is authorized to do this action
            var specification = new GetManagerAuthorizedProjectsSpecification(projectId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project), "user is not authorized to do this action");

            project.UpdateTask(taskId, requestDto.AssignedTranslatorId, requestDto.Title, requestDto.Description,
                requestDto.DueDate);

            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(project.ToProjectResponse(), true, "Task updated",
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
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }

    public async Task<ServiceResult<bool>> MarkTaskAsCompleted(int taskId, ClaimsPrincipal user)
    {
        try
        {
            var specification = new GetTranslatorsAuthorizedTasksSpecification(taskId, user.GetUserId());
            var project = await _projectRepository.GetByIdAsync(specification);

            Guard.Against.Null(project, nameof(project),
                "user is not authorized to do this action or project not exists");

            project.MarkTaskAsCompleted(taskId);
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

    public async Task<ServiceResult<TaskResponse>> GetTasksAssignedToTranslator(SharedParamFilter input)
    {
        try
        {
            var spec = new TasksForTranslatorSpecification(input);
            var specCount = new TasksForTranslatorSpecificationCount(input);
            var tasks = await _projectTaskRepository.ListAsync(spec, specCount);
            var result = tasks.list.Select(t => t.ToTaskResponse()).ToList();

            return new ServiceResult<TaskResponse>(result, true, "Tasks Retrieved", HttpStatusCode.OK,
                tasks.totalCount);
        }
        catch (DomainException ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<TaskResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
            return new ServiceResult<TaskResponse>(false, ex.Message, HttpStatusCode.InternalServerError);
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

            Guard.Against.Null(task, nameof(task), "task not found");

            //Complete Tasks Are Not Modifiable 
            if (task.Status == ProjectStatus.Completed)
            {
                throw new CompleteTasksAreNotModifiableException();
            }

            Guard.Against.Null(task, nameof(task));

            project.RemoveTask(task);

            // Update the project in the repository
            await _projectRepository.UpdateAsync(project);
            return new ServiceResult<ProjectResponse>(project.ToProjectResponse(), true, "Task Removed",
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
            return new ServiceResult<ProjectResponse>(false, ex.Message, HttpStatusCode.BadRequest);
        }
    }
}