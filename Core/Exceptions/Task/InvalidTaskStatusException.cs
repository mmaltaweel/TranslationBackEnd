namespace Core.Exceptions;

public class InvalidTaskStatusException : DomainException
{ 
    public InvalidTaskStatusException(Core.Enities.ProjectAggregate.ProjectStatus currentEStatus) : base($"Task cannot be started because it is already in {currentEStatus} status.")
    {
    }
}