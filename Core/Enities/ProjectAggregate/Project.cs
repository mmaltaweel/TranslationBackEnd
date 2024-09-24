using Core.Interfaces;
using Core.Exceptions;

namespace Core.Enities.ProjectAggregate;

public class Project : BaseEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public ProjectStatus Status { get; private set; }

    public string ProjectManagerId { get; set; }
    public virtual User ProjectManager { get; private set; }

    public virtual List<ProjectTask> Tasks { get; private set; } = new List<ProjectTask>();
  


    public Project()
    {
        // For EF Core 
    }

    public Project(string name, string description, DateTime startDate, DateTime endDate, User projectManager)
    {
        // Validate parameters
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidUserNameException(name, string.Empty); // Invalid name exception

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException(nameof(description), "Description cannot be null or empty.");

        if (endDate < startDate)
            throw new InvalidProjectDatesException(startDate, endDate);
        
        //check if the user a manager to CreateProject
        if (projectManager.Role != UserRole.ProjectManager)
            throw new InvalidUserRoleForManagingProjectException(projectManager.Role);

        Name = name;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Status = ProjectStatus.NotStarted;
        ProjectManager = projectManager ?? throw new NullUserRoleException();
        ProjectManagerId= projectManager.Id;
        Tasks = new List<ProjectTask>();
    }

    public void UpdateManager(User newManager)
    {
        ProjectManager = newManager ?? throw new ArgumentNullException(nameof(newManager));
        ProjectManagerId= newManager.Id;
    }

    public void UpdateStartDate(DateTime newStartDate)
    {
        if (newStartDate > EndDate)
            throw new InvalidOperationException("Start date cannot be after the end date.");

        StartDate = newStartDate;
    }

    public void UpdateEndDate(DateTime newEndDate)
    {
        if (newEndDate < StartDate)
            throw new InvalidOperationException("End date cannot be before the start date.");

        EndDate = newEndDate;
    }
    public ProjectTask CreateTask(string title, string description, DateTime dueDate, User assignedTranslatorId)
    {
        // Create a new task
        var newTask = new ProjectTask(title,description, dueDate,assignedTranslatorId);

        // Add the task to the project's task list
        Tasks.Add(newTask);

        // Return the newly created task
        return newTask;
    }
    public void UpdateTask(int taskId, string newAssigneeId, string newTitle, string newDescription, DateTime? newDueDate, string managerId)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
        {
            throw new ArgumentException($"Task with ID {taskId} not found in the project.");
        }

        // Update the task's properties if new values are provided
        if (!string.IsNullOrWhiteSpace(newAssigneeId))
        {
            task.UpdateAssignee(newAssigneeId, managerId);
        }

        if (!string.IsNullOrWhiteSpace(newTitle))
        {
            task.UpdateTitle(newTitle);
        }

        if (!string.IsNullOrWhiteSpace(newDescription))
        {
            task.UpdateDescription(newDescription);
        }

        if (newDueDate.HasValue)
        {
            task.UpdateDueDate(newDueDate.Value);
        }
    }
    public void RemoveTask(ProjectTask task)
    {
        if (!Tasks.Contains(task))
        {
            throw new InvalidOperationException("Task does not exist in the project.");
        }

        Tasks.Remove(task);
    }

    public void StartProject()
    {
        if (Status != ProjectStatus.NotStarted)
            throw new ProjectAlreadyStartedException();

        Status = ProjectStatus.InProgress;
    }

    public void CompleteProject()
    {
        if (Tasks.All(t => t.Status == TaskEStatus.Completed))
        {
            Status = ProjectStatus.Completed;
        }
        else
        {
            throw new IncompleteTasksException();
        }
    }
}