namespace Core.Exceptions;

public class InvalidUserEmailException : Exception
{
    public InvalidUserEmailException(string email) : base($"Invalid email address: '{email}'.")
    {
    }
}