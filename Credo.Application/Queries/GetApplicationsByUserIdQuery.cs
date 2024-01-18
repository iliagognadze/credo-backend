using System.Security.Claims;
using AutoMapper;
using Credo.Application.Extensions;
using Entities.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Repository.Contracts;
using Shared.DTOs;

namespace Credo.Application.Queries;

public class GetApplicationsByUserIdQuery : IRequest<List<ApplicationDto>>
{
    public IEnumerable<Claim> Claims { get; }

    public GetApplicationsByUserIdQuery(IEnumerable<Claim> claims)
    {
        Claims = claims;
    }
}

class GetApplicationsByUserIdQueryHandler : IRequestHandler<GetApplicationsByUserIdQuery, List<ApplicationDto>>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetApplicationsByUserIdQueryHandler(
        IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<List<ApplicationDto>> Handle(GetApplicationsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = request.Claims.GetClaimValue(UserClaims.UserId);
        
        if (!int.TryParse(userId, out var userIdParsed)) 
            throw new Exception("Invalid user id from access token.");

        var applications = await _repository.Application.GetByUserIdAsync(userIdParsed, cancellationToken);

        return _mapper.Map<List<ApplicationDto>>(applications);
    }
}