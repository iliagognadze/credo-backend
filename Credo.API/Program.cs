using Credo.API.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureRepository();
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

#region users

var users = app.MapGroup("api/users");

users.MapPost("", async (
    [FromServices] IMediator mediator,
    [FromBody] UserForCreationDto userForCreation) => 
{
    
});

#endregion

#region applications

var applications = app.MapGroup("api/applications");

applications.MapGet("", async (
    [FromServices] IMediator mediator) =>
{
    
});

applications.MapGet("{id:int}", async (
    [FromServices] IMediator mediator,
    int id) =>
{
    
});

applications.MapPost("", async (
    [FromServices] IMediator mediator,
    [FromBody] ApplicationForCreationDto applicationForCreation) =>
{

});

#endregion

app.Run();