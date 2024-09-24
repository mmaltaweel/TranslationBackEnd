namespace Core.Exceptions;

public class InvalidTaskTitleException : DomainException
{
    public InvalidTaskTitleException() : base($"Task title is invalid or empty.")
    {
    }
}