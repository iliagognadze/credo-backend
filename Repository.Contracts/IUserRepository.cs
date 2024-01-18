using Entities.Models;

namespace Repository.Contracts;

public interface IUserRepository
{
    Task<User?> GetAsync(string email, string password, CancellationToken token, bool trackChanges = false);
    Task CreateAsync(User user);
}