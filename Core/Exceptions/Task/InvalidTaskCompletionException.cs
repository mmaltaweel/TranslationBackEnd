namespace Core.Exceptions;

public class InvalidTaskCompletionException : DomainException
{
    public InvalidTaskCompletionException() : base("Task must have an assigned translator.")
    {
    }
}