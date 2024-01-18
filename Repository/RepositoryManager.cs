using Repository.Contracts;

namespace Repository;

public class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<IApplicationRepository> _applicationRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        _applicationRepository = new Lazy<IApplicationRepository>(() => new ApplicationRepository(repositoryContext));
    }

    public IUserRepository User => _userRepository.Value;
    public IApplicationRepository Application => _applicationRepository.Value;

    public async Task SaveChangesAsync() => await _repositoryContext.SaveChangesAsync();
}