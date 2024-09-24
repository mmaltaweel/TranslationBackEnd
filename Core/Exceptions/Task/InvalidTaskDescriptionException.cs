namespace Core.Exceptions;

 
public class InvalidTaskDescriptionException : DomainException
{
    public InvalidTaskDescriptionException() : base($"Description is invalid or empty.")
    {
    }
}