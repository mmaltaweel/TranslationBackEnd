namespace Core.Exceptions;

public class IncompleteTasksException : DomainException
{
    public IncompleteTasksException() : base("Project cannot be completed until all tasks are done.")
    {
        
    }
}