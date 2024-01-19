namespace Entities.Exceptions;

public class UserWithProvidedPrivateNumberAlreadyExistsException : BadRequestException
{
    public UserWithProvidedPrivateNumberAlreadyExistsException() : base("USER_WITH_PROVIDED_PRIVATE_NUMBER_ALREADY_EXISTS")
    {
    }
}