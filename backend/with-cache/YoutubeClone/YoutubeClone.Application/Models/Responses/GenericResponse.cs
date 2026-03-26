using YoutubeClone.Shared.Helpers;

namespace YoutubeClone.Application.Models.Responses
{
    public class GenericResponse<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; } = DateTimeHelper.UtcNow();
    }
}
