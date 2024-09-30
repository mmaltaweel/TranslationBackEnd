namespace Core.Exceptions;

public class CompleteTasksAreNotModifiableException:DomainException
{
    public CompleteTasksAreNotModifiableException() : base($"Complete Tasks are not modifiable")
    {
    }
}