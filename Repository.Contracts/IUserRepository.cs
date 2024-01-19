using Entities.Models;

namespace Repository.Contracts;

public interface IUserRepository
{
    Task<User?> GetAsync(string email, string password, CancellationToken token, bool trackChanges = false);
    Task<User?> GetAsync(int id, CancellationToken token, bool trackChanges = false);
    Task<User?> GetByEmailAsync(string email, CancellationToken token, bool trackChanges = false);
    Task<User?> GetByPrivateNumberAsync(string privateNumber, CancellationToken token, bool trackChanges = false);
    Task CreateAsync(User user);
}