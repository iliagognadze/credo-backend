namespace Repository.Contracts;

public interface IRepositoryManager
{
    IUserRepository User { get; }
    IApplicationRepository Application { get; }
    
    Task SaveChangesAsync();
}