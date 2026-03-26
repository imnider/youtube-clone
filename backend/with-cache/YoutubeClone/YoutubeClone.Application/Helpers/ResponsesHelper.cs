using YoutubeClone.Application.Models.Responses;

namespace YoutubeClone.Application.Helpers
{
    public static class ResponsesHelper
    {
        public static GenericResponse<T> Create<T>(T data, string message = "Solicitud exitosa")
        {
            var response = new GenericResponse<T>
            {
                Message = message,
                Data = data
            };
            return response;
        }
    }
}
