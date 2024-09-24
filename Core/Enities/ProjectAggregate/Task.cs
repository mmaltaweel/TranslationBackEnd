using Core.Exceptions;

namespace Core.Enities.ProjectAggregate;
public class ProjectTask : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public TaskEStatus Status { get; private set; }
    
    public string AssignedTranslatorId { get; private set; }
    public virtual User AssignedTranslator { get; private set; }
    
    public int ProjectId { get; private set; }
    public virtual Project Project { get; private set; }
 

    public ProjectTask() { } // For EF Core

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
        Status = TaskEStatus.NotStarted;
    }

    public void StartTask()
    {
        if (Status != TaskEStatus.NotStarted)
            throw new InvalidTaskStatusException(Status);

        Status = TaskEStatus.InProgress;
    }
    public void CompleteTask()
    {
        if (Status != TaskEStatus.InProgress)
            throw new InvalidTaskCompletionException();

        Status = TaskEStatus.Completed;
    }
    public void UpdateDueDate(DateTime newDueDate)
    { 
        DueDate = newDueDate;
    }
    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new InvalidTaskTitleException();
        
        Title = newTitle;
    }
    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new InvalidTaskDescriptionException();

        Description = newDescription;
    }
    public void UpdateAssignee(string newAssigneeId, string managerId) // Manager ID is required for validation
    {
        // Here you can check if the manager has the right privileges, if needed
        // For now, we assume if this method is called, it’s from a manager
        AssignedTranslatorId = newAssigneeId; // Update the assignee
    }
}