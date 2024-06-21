using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;

namespace JWT.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder appBuilder)
    {
        appBuilder.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                // Implement exception handling here
                
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                
                var json = JsonSerializer.Serialize(new { 
                    title = "Something went wrong!",
                    status = (int)HttpStatusCode.InternalServerError,
                    errorCode =StatusCodes.Status400BadRequest
                });

                await context.Response.WriteAsync(json, cancellationToken: CancellationToken.None);
            });
        });
    }
}