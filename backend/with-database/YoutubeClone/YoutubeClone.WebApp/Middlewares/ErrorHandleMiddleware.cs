using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Models.Responses;
using YoutubeClone.Domain.Exceptions;

namespace YoutubeClone.WebApp.Middlewares
{
    public class ErrorHandleMiddleware : IMiddleware
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
                await context.Response.WriteAsJsonAsync(ManageException(context, exception, StatusCodes.Status500InternalServerError));
            }
        }

        public GenericResponse<string> ManageException(HttpContext context, Exception exception, int statusCode)
        {
            var response = ResponsesHelper.Create(
                data: exception.Message,
                message: exception.Message,
                errors: [exception.Message]);
            context.Response.StatusCode = statusCode;
            return response;
        }
    }
}