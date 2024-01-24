using System.Text;
using Credo.API.Extensions;
using Credo.Application.Commands;
using Credo.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
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
builder.Host.ConfigureLogger(builder);
builder.Services.ConfigureHttpLogging();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        corsPolicyBuilder => corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

app.UseHttpLogging();

app.UseCors("CorsPolicy");

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

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
    Results.Ok(await mediator.Send(new GetApplicationsByUserIdQuery(httpContext.User.Claims))))
    .RequireAuthorization();

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

applications.MapDelete("/{id:int}", async (
        int id,
        HttpContext httpContext,
        [FromServices] IMediator mediator) =>
    {
        var claims = httpContext.User.Claims;
        
        await mediator.Send(new DeleteApplicationCommand(id, claims));

        return Results.NoContent();
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

app.UseCors("CorsPolicy");

app.Run();