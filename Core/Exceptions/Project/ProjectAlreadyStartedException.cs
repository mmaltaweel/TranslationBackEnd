namespace Core.Exceptions;

public class ProjectAlreadyStartedException : DomainException
{
    public ProjectAlreadyStartedException() : base("The project has already started or is completed.")
    {
    }
}