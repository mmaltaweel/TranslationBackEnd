namespace Core.Exceptions;

public class NullProjectException : DomainException
{
    public NullProjectException() : base("Project cannot be null.")
    {
    }
}