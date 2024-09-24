namespace Core.Exceptions;

public class ProjectNotFoundException: DomainException
{
    public ProjectNotFoundException(int projectId) : base($"No Project found with id {projectId}")
    {
    }
}