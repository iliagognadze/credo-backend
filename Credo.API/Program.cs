using Credo.API.Extensions;
using Credo.Application;
using Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureSqlContext(builder.Configuration);
OptionsServiceCollectionExtensions.ConfigureOptions(builder.Services, builder.Configuration);
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

app.Run();