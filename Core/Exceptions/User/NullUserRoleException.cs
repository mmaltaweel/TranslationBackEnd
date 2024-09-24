namespace Core.Exceptions;

public class NullUserRoleException : Exception
{
    public NullUserRoleException(): base("User role cannot be null.")
    {
    }
}