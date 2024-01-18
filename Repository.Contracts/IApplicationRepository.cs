using Entities.Models;

namespace Repository.Contracts;

public interface IApplicationRepository
{
    Task<List<Application>> GetAsync(CancellationToken token, bool trackChanges = false);
    Task<Application?> GetAsync(int id, CancellationToken token, bool trackChanges = false);
    Task CreateAsync(Application application);
}