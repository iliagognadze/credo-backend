using System.Security.Claims;
using AutoMapper;
using Entities.Constants;
using MediatR;
using Repository.Contracts;
using Shared.DTOs;

namespace Credo.Application.Commands;

public class CreateApplicationCommand : IRequest<ApplicationDto>
{
    public ApplicationForCreationDto ApplicationForCreation { get; }
    public IEnumerable<Claim> Claims { get; }

    public CreateApplicationCommand(
        ApplicationForCreationDto applicationForCreation,
        IEnumerable<Claim> claims)
    {
        ApplicationForCreation = applicationForCreation;
        Claims = claims;
    }
}

public class CreateApplicationCommandHandler : IRequestHandler<CreateApplicationCommand, ApplicationDto>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CreateApplicationCommandHandler(
        IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<ApplicationDto> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        var userId = request.Claims
            .FirstOrDefault(c => c.Type == UserClaims.UserId)?.Value;
        
        if (userId is null || !int.TryParse(userId, out var userIdParsed)) 
            throw new Exception("Invalid user id from access token.");
        
        var newApplication = _mapper.Map<Entities.Models.Application>(request.ApplicationForCreation);

        newApplication.UserId = userIdParsed;
        newApplication.Status = ApplicationStatus.Sent;

        await _repository.Application.CreateAsync(newApplication);

        await _repository.SaveChangesAsync();

        return _mapper.Map<ApplicationDto>(newApplication);
    }
}