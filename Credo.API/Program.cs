using System.Text;
using Credo.API.Extensions;
using Credo.Application.Commands;
using Credo.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization();

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureRepository();
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureOptions(builder.Configuration);
builder.Services.ConfigureMediator();
builder.Services.ConfigureSwagger();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region authorization

var auth = app.MapGroup("api/auth").WithTags("auth");

auth.MapPost("", async (
    [FromServices] IMediator mediator,
    [FromBody] AuthDto authorization) => 
    Results.Ok(await mediator.Send(new GenerateAccessTokenCommand(authorization))));

#endregion

#region users

var users = app.MapGroup("api/users").WithTags("users");

users.MapPost("", async (
            [FromServices] IMediator mediator,
            [FromBody] UserForCreationDto userForCreation) => 
        Results.Created("users", await mediator.Send(new CreateUserCommand(userForCreation))))
    .Produces<UserDto>();

#endregion

#region applications

var applications = app.MapGroup("api/applications").WithTags("applications");

applications.MapGet("", async (
        HttpContext httpContext,
        [FromServices] IMediator mediator) =>
    Results.Ok(await mediator.Send(new GetApplicationsByUserIdQuery(httpContext.User.Claims))));

applications.MapGet("{id:int}", async (
    [FromServices] IMediator mediator,
    int id) =>
{
    
}).RequireAuthorization();

applications.MapPost("", async (
            HttpContext httpContext,
            [FromServices] IMediator mediator,
            [FromBody] ApplicationForCreationDto applicationForCreation) =>
    {
        var claims = httpContext.User.Claims;
        
        return Results.Created("applications",
            await mediator.Send(new CreateApplicationCommand(applicationForCreation, claims)));
    })
    .RequireAuthorization()
    .Produces<ApplicationDto>();

applications.MapPut("{id:int}", async (
        HttpContext httpContext,
        int id,
        [FromServices] IMediator mediator,
        [FromBody] ApplicationForUpdateDto applicationForUpdate) =>
    {
        var claims = httpContext.User.Claims;

        await mediator.Send(new UpdateApplicationCommand(id, applicationForUpdate, claims));

        return Results.NoContent();
    })
    .RequireAuthorization();

#endregion

app.UseAuthentication();
app.UseAuthorization();

app.Run();