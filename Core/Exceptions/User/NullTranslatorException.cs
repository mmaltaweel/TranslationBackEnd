namespace Core.Exceptions;

public class NullTranslatorException : Exception
{
    public NullTranslatorException(): base("Task must have an assigned translator.")
    {
    }
}