using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts;

namespace Repository;

public class ApplicationRepository : RepositoryBase<Application>, IApplicationRepository
{
    public ApplicationRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<List<Application>> GetByUserIdAsync(int userId, CancellationToken token, bool trackChanges = false) =>
        await FindByCondition(application => application.UserId == userId, trackChanges)
            .ToListAsync(token);

    public async Task<Application?> GetAsync(int id, CancellationToken token, bool trackChanges = false) =>
        await FindByCondition(application => application.Id == id, trackChanges)
            .FirstOrDefaultAsync(token);

    public async Task CreateAsync(Application application) => await Create(application);
}