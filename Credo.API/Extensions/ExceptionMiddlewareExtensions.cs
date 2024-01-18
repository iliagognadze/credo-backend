using Entities.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Shared.HttpModels;

namespace Credo.API.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this WebApplication app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature is not null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    var error = new ErrorDetails
                        { StatusCode = context.Response.StatusCode, Message = contextFeature.Error.Message };

                    if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
                    {
                        logger.LogError(contextFeature.Error, "SOMETHING_WENT_WRONG");
                        error.Message = "INTERNAL_ERROR";
                    }
                    else
                        logger.LogWarning("{customError}", error);

                    await context.Response.WriteAsJsonAsync(error);
                }
            });
        });
    }
}