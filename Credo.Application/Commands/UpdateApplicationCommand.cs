using System.Security.Claims;
using AutoMapper;
using Credo.Application.Extensions;
using Entities.Constants;
using Entities.Exceptions;
using Entities.Models;
using MediatR;
using Repository.Contracts;
using Shared.DTOs;

namespace Credo.Application.Commands;

public class UpdateApplicationCommand : IRequest
{
    public int ApplicationId { get; }
    public ApplicationForUpdateDto ApplicationForUpdate { get; }
    public IEnumerable<Claim> Claims { get; }

    public UpdateApplicationCommand(
        int applicationId,
        ApplicationForUpdateDto applicationForUpdate,
        IEnumerable<Claim> claims)
    {
        ApplicationId = applicationId;
        ApplicationForUpdate = applicationForUpdate;
        Claims = claims;
    }
}

class UpdateApplicationCommandHandler : IRequestHandler<UpdateApplicationCommand>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public UpdateApplicationCommandHandler(
        IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task Handle(UpdateApplicationCommand request, CancellationToken cancellationToken)
    {
        var userId = request.Claims.GetClaimValue(UserClaims.UserId);

        if (!int.TryParse(userId, out var userIdParsed))
            throw new Exception("Invalid user id provided in token.");

        await CheckUser(userIdParsed, cancellationToken);

        var application = await _repository.Application.GetAsync(request.ApplicationId, cancellationToken, true);

        if (application?.Status is ApplicationStatus.Approved or ApplicationStatus.Processing)
            throw new Exception("You are not able to edit application.");
            
        if (application?.UserId != userIdParsed) 
            throw new Exception("Application could not be found or is not created by provided user.");

        _mapper.Map(request.ApplicationForUpdate, application);

        await _repository.SaveChangesAsync();
    }
    
    private async Task<User> CheckUser(int id, CancellationToken cancellationToken)
    {
        var user = await _repository.User.GetAsync(id, cancellationToken);

        return user ?? throw new UserNotFoundException(id);
    }
}