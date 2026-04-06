using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Domain.Exceptions;
using YoutubeClone.Shared.Constants;

namespace YoutubeClone.WebApp.Middlewares
{
    public class ErrorHandleMiddleware(ILogger<ErrorHandleMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException exception)
            {
                await context.Response.WriteAsJsonAsync(ManageException(context, exception, StatusCodes.Status404NotFound));
            }
            catch (BadRequestException exception)
            {
                await context.Response.WriteAsJsonAsync(ManageException(context, exception, StatusCodes.Status400BadRequest));
            }
            catch (Exception exception)
            {
                var traceId = Guid.NewGuid();
                var message = ResponseConstants.ERROR_UNEXPECTED(traceId.ToString());

                logger.LogCritical("Se generó una excepción no controlada, con el traceId: {traceId}. Excepción: {exception}", traceId, exception);

                await context.Response.WriteAsJsonAsync(ManageException(context, exception, StatusCodes.Status500InternalServerError, message));
            }
        }

        public GenericResponse<string> ManageException(HttpContext context, Exception exception, int statusCode, string? message = null)
        {
            var response = ResponsesHelper.Create(
                data: message ?? exception.Message,
                message: message ?? exception.Message,
                errors: [message ?? exception.Message]);
            context.Response.StatusCode = statusCode;
            return response;
        }
    }
}