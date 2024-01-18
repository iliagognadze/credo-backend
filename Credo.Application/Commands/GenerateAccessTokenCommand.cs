using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Entities.Exceptions;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Contracts;
using Shared.DTOs;
using Shared.Options;

namespace Credo.Application.Commands;

public class GenerateAccessTokenCommand : IRequest<AccessTokenDto>
{
    public AuthDto Authorization { get; }

    public GenerateAccessTokenCommand(AuthDto authorization)
    {
        Authorization = authorization;
    }
}

public class GenerateAccessTokenCommandHandler : IRequestHandler<GenerateAccessTokenCommand, AccessTokenDto>
{
    private readonly IRepositoryManager _repository;
    private readonly JwtSettings _jwtSettings;

    public GenerateAccessTokenCommandHandler(
        IRepositoryManager repository,
        IOptions<JwtSettings> jwtSettingsOptions)
    {
        _repository = repository;
        _jwtSettings = jwtSettingsOptions.Value;
    }
    
    public async Task<AccessTokenDto> Handle(GenerateAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await CheckUser(request.Authorization.Email, request.Authorization.Password, cancellationToken);
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.Now.AddMinutes(120);
        
        var token = new JwtSecurityToken(
            _jwtSettings.Issuer!,
            _jwtSettings.Audience!,
            null,
            expires: expires,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        if (accessToken is null) throw new Exception("Could not generate access token.");

        return new AccessTokenDto
        {
            AccessToken = accessToken,
            AccessType = "Bearer",
            ExpiresAt = expires
        };
    }

    private async Task<User> CheckUser(string email, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = HashPassword(password);

        var user = await _repository.User.GetAsync(email, hashedPassword, cancellationToken);

        return user ?? throw new UserInvalidCredentialsException();
    }
    
    private static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(128 / 8); 
        
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password!,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
    }
}