using Core.Enities.ProjectAggregate;

namespace Core.Exceptions;

public class InvalidUserRoleForTaskAssignmentException : Exception
{
    public InvalidUserRoleForTaskAssignmentException(UserRole role) : base($"User with role '{role}' is not authorized to be assigned a task.")
    {
    }
}