namespace Entities.Exceptions;

public class NotAbleToDeleteApplicationException : ForbiddenException
{
    public NotAbleToDeleteApplicationException(int userId, int applicationId) 
        : base($"Application with id: {applicationId} can not be deleted by user with id: {userId}")
    {
    }
}