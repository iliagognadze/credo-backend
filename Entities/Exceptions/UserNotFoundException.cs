namespace Entities.Exceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(int id) : base($"User with id: {id} could not be found.")
    {
    }
}