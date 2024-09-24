namespace Core.Exceptions;

public class NullProjectManagerException : DomainException
{
    public NullProjectManagerException() : base("Project must have a manager.")
    {
    }
}