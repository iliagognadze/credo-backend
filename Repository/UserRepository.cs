using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<User?> GetAsync(string email, string password, CancellationToken token, bool trackChanges = false) =>
        await FindByCondition(user => user.Email == email && user.Password == password, trackChanges)
            .FirstOrDefaultAsync(token);
    
    public async Task<User?> GetAsync(int id, CancellationToken token, bool trackChanges = false) =>
        await FindByCondition(user => user.Id == id, trackChanges)
            .FirstOrDefaultAsync(token);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken token, bool trackChanges = false) =>
        await FindByCondition(user => user.Email == email, trackChanges)
            .FirstOrDefaultAsync(token);

    public async Task<User?> GetByPrivateNumberAsync(string privateNumber, CancellationToken token, bool trackChanges = false) =>
        await FindByCondition(user => user.PrivateNumber == privateNumber, trackChanges)
            .FirstOrDefaultAsync(token);

    public async Task CreateAsync(User user) => await Create(user);
}