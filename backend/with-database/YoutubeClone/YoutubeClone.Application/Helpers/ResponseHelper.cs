using YoutubeClone.Application.Models.Responses;

namespace YoutubeClone.Application.Helpers
{
    public static class ResponseHelper
    {
        public static GenericResponse<T> Create<T>(T data, List<string>? errors = null, string? message = null)
        {
            var response = new GenericResponse<T>
            {
                Message = message ?? "Solicitud exitosa",
                Data = data,
                Errors = errors ?? []
            };
            return response;
        }
    }
}
