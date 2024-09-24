namespace Core.Exceptions;

public class InvalidProjectNameException : DomainException
{
    public InvalidProjectNameException(string name) : base($"Project name '{name}' is invalid or empty.")
    {
    }
}