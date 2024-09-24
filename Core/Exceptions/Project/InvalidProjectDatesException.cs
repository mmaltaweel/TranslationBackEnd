namespace Core.Exceptions;

public class InvalidProjectDatesException : DomainException
{
    public InvalidProjectDatesException(DateTime startDate, DateTime endDate) : base($"End date {endDate} cannot be earlier than start date {startDate}.")
    {
    }
}