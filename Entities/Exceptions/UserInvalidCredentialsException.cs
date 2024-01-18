namespace Entities.Exceptions;

public class UserInvalidCredentialsException : UnauthorizedException
{
    public UserInvalidCredentialsException() : base("Invalid user credentials.")
    {
        
    }
}