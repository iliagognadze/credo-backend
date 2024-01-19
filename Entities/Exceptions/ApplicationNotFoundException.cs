namespace Entities.Exceptions;

public class ApplicationNotFoundException : NotFoundException
{
    public ApplicationNotFoundException(int id) : base($"Application with id: {id} could not be found.")
    {
    }
}