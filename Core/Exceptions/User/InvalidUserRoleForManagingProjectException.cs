using Core.Enities.ProjectAggregate;

namespace Core.Exceptions;

public class InvalidUserRoleForManagingProjectException : Exception
{
    public InvalidUserRoleForManagingProjectException(UserRole role) : base($"User with role '{role}' is not authorized to manage a project.")
    {
    }
}