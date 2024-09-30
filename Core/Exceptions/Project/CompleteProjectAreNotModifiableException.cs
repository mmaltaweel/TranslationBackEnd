namespace Core.Exceptions;

public class CompleteProjectAreNotModifiableException:DomainException
{
    public CompleteProjectAreNotModifiableException() : base($"Complete projects are not modifiable")
    {
    }
}