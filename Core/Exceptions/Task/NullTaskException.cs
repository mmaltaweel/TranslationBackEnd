namespace Core.Exceptions;

public class NullTaskException : DomainException
{
    public NullTaskException() : base("Task cannot be null.")
    {
    }
}