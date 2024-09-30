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

    public string ProjectManagerId { get; private set; }
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
        Status = ProjectStatus.InProgress;
        ProjectManager = projectManager ?? throw new NullUserRoleException();
        ProjectManagerId = projectManager.Id;
        Tasks = new List<ProjectTask>();
    }

    public void UpdateProject(DateTime newStartDate, DateTime newEndDate, string newName, string newDescription)
    {
        
        //if the project is completed then update is declined
        if(this.Status==ProjectStatus.Completed)
        {
            throw new CompleteProjectAreNotModifiableException();
        }
        if (this.ProjectManager.Role != UserRole.ProjectManager)
            throw new InvalidUserRoleForManagingProjectException(this.ProjectManager.Role);

        // Update the start date

        if (newStartDate > EndDate)
            throw new InvalidOperationException("Start date cannot be after the end date.");
        StartDate = newStartDate;


        // Update the end date and do validation end>start

        if (newEndDate < StartDate)
            throw new InvalidOperationException("End date cannot be before the start date.");
        EndDate = newEndDate;

        //update the name if not empty
        if (!string.IsNullOrWhiteSpace(newName))
        {
            Name = newName;
        }

        //update the description if not empty
        if (!string.IsNullOrWhiteSpace(newDescription))
        {
            Description = newDescription;
        }
    }

    #region status domain logic

    /* Project and Task status states Business Assumptions for UpdateProjectStatus method:
        1- All new projects and tasks are initialized with the status InProgress. ok
        2- Project Completion Criteria:
           a- The manager manually selects Mark as Completed we need to check if all tasks are compeleted as well before changing the project status done
           b- All associated tasks have the status Completed this should be done in service layer by translator action that take action on a specifc task.
           c- Attempting to mark a project as Completed when one or more tasks are still InProgress will result in an exception. done
        3- Overdue Status:
            A project will be marked as Overdue if:
              a- At least one associated task has the status Overdue.
              b- The project's end date has passed the current date.
     */

    public void MarkProjectAsCompleted()
    {
        if (this.ProjectManager.Role != UserRole.ProjectManager)
            throw new InvalidUserRoleForManagingProjectException(this.ProjectManager.Role);

        this.Tasks.ForEach(x => x.MarkTaskAsCompleted()) ; //  all tasks will be completed before update the project status
        this.Status = ProjectStatus.Completed;
    }

    public void MarkProjectAsOverDue()
    {
        if (EndDate < DateTime.Now || this.Tasks.Any(x => x.Status == ProjectStatus.Overdue))
        {
            this.Status = ProjectStatus.Overdue;
        }
    }
    #endregion

    // Since DDD principles are applied here, the Project is considered the aggregate root,
    // and it alone is responsible for managing the state changes of its child entities.

    #region task domain logic

    public ProjectTask CreateTask(string title, string description, DateTime dueDate, User assignedTranslatorId)
    {
        // Create a new task
        var newTask = new ProjectTask(title, description, dueDate, assignedTranslatorId);

        // Add the task to the project's task list
        Tasks.Add(newTask);

        // Return the newly created task
        return newTask;
    }

    public void UpdateTask(int taskId, string newAssigneeId, string newTitle, string newDescription,
        DateTime newDueDate)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
        {
            throw new ArgumentException($"Task with ID {taskId} not found in the project.");
        }

        task.Update(newDueDate, newTitle, newDescription, newAssigneeId);
    }
    public void MarkTaskAsCompleted(int taskId)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task == null)
        {
            throw new ArgumentException($"Task with ID {taskId} not found in the project.");
        }
        task.MarkTaskAsCompleted();
        
        //check if all tasks are completed to set project completed as well
        if (Tasks.All(x => x.Status == ProjectStatus.Completed))
        {
            this.Status= ProjectStatus.Completed;
        }
    }
    public void RemoveTask(ProjectTask task)
    {
        var removedTask = Tasks.FirstOrDefault(x => x.Id == task.Id);
        if (removedTask is null)
        {
            throw new InvalidOperationException("Task does not exist in the project.");
        }
        else
        {
            //if the task is completed then update is declined
            if(removedTask.Status==ProjectStatus.Completed)
            {
                throw new CompleteProjectAreNotModifiableException();
            }
        }
        Tasks.Remove(task);
    }
    #endregion
}