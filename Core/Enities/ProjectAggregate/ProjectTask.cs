using Core.Exceptions;

namespace Core.Enities.ProjectAggregate;

public class ProjectTask : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public ProjectStatus Status { get; private set; }


    public string AssignedTranslatorId { get; private set; }
    public virtual User AssignedTranslator { get; private set; }

    public int ProjectId { get; private set; }
    public virtual Project Project { get; private set; }


    public ProjectTask()
    {
    } // For EF Core

    public ProjectTask(string title, string description, DateTime dueDate, User assignedTranslator)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new InvalidTaskTitleException();

        if (string.IsNullOrWhiteSpace(description))
            throw new InvalidTaskDescriptionException();

        if (assignedTranslator.Role != UserRole.Translator)
            throw new InvalidUserRoleForTaskAssignmentException(assignedTranslator.Role);

        Title = title;
        Description = description;
        DueDate = dueDate;
        AssignedTranslator = assignedTranslator ?? throw new NullTranslatorException();
        Status = ProjectStatus.InProgress;
    }

    public void Update(DateTime newDueDate, string newTitle, string newDescription, string newAssigneeId)
    {
        if (this.Project.ProjectManager.Role != UserRole.ProjectManager)
            throw new InvalidUserRoleForTaskAssignmentException(this.Project.ProjectManager.Role);

        DueDate = newDueDate;

        if (string.IsNullOrWhiteSpace(newTitle))
            throw new InvalidTaskTitleException();

        Title = newTitle;

        if (string.IsNullOrWhiteSpace(newAssigneeId))
            throw new InvalidTaskDescriptionException();

        Description = newDescription;

        if (string.IsNullOrWhiteSpace(newAssigneeId))
            throw new InvalidTaskDescriptionException();

        AssignedTranslatorId = newAssigneeId;
        
        UpdateTaskStatus(Status);
    }

    public void UpdateTaskStatus(ProjectStatus newStatus)
    {
        //assumption: task must be InProgress before mark it as completed
        if (Status != ProjectStatus.InProgress && newStatus!=ProjectStatus.Completed)
            throw new ProjectAlreadyStartedException();
          
        this.Status = newStatus;
    }
    public void CompleteTask()
    {
        if (this.AssignedTranslator.Role != UserRole.Translator)
            throw new InvalidUserRoleForTaskAssignmentException(this.AssignedTranslator.Role);

        if (Status != ProjectStatus.InProgress)
            throw new InvalidTaskCompletionException();

        Status = ProjectStatus.Completed;
    }

    public void MarkTaskAsCompleted()
    {
        this.Status=ProjectStatus.Completed;
    }
}