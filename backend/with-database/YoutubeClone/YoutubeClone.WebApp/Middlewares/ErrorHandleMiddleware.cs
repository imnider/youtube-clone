using YoutubeClone.Application.Helpers;
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
                var response = ResponsesHelper.Create(exception.Message);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (BadRequestException exception)
            {
                var response = ResponsesHelper.Create(exception.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception exception)
            {
                var response = ResponsesHelper.Create(exception.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}