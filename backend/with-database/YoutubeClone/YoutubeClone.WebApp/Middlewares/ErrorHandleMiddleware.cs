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

                throw;
            }
        }
    }
}
