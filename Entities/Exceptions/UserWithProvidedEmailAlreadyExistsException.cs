namespace Entities.Exceptions;

public class UserWithProvidedEmailAlreadyExistsException : BadRequestException
{
    public UserWithProvidedEmailAlreadyExistsException() : base("USER_WITH_PROVIDED_EMAIL_ALREADY_EXISTS")
    {
    }
}