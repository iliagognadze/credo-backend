using System.Security.Claims;
using AutoMapper;
using Credo.Application.Extensions;
using Entities.Constants;
using Entities.Exceptions;
using MediatR;
using Repository.Contracts;

namespace Credo.Application.Commands;

public class DeleteApplicationCommand : IRequest
{
    public int ApplicationId { get; }
    public IEnumerable<Claim> Claims { get; }

    public DeleteApplicationCommand(
        int applicationId,
        IEnumerable<Claim> claims)
    {
        ApplicationId = applicationId;
        Claims = claims;
    }
}

class DeleteApplicationCommandHandler : IRequestHandler<DeleteApplicationCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public DeleteApplicationCommandHandler(
        IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
    {
        var userId = request.Claims.GetClaimValue(UserClaims.UserId);

        if (!int.TryParse(userId, out var userIdParsed))
            throw new Exception("Invalid user id.");

        var application = await _repository.Application.GetAsync(request.ApplicationId, cancellationToken);
        if (application is null) throw new ApplicationNotFoundException(userIdParsed);
        
        if (application.UserId != userIdParsed) 
            throw new Exception("No permission to delete this application.");

        _repository.Application.Remove(application);

        await _repository.SaveChangesAsync();
    }
}