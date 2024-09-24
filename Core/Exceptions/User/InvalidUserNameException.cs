namespace Core.Exceptions;

public class InvalidUserNameException : Exception
{
    public InvalidUserNameException(string firstName, string lastName) : base($"Invalid user name: '{firstName} {lastName}'.")
    {
    }
}