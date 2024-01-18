using AutoMapper;
using Credo.Application.Extensions;
using Entities.Models;
using MediatR;
using Repository.Contracts;
using Shared.DTOs;

namespace Credo.Application.Commands;

public class CreateUserCommand : IRequest<UserDto>
{
    public UserForCreationDto UserForCreation { get; }

    public CreateUserCommand(
        UserForCreationDto userForCreation)
    {
        UserForCreation = userForCreation;
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = _mapper.Map<User>(request.UserForCreation);

        newUser.Password = newUser.Password.HashPassword();

        await _repository.User.CreateAsync(newUser);

        await _repository.SaveChangesAsync();

        return _mapper.Map<UserDto>(newUser);
    }
}