using YoutubeClone.Shared.Helpers;

namespace YoutubeClone.Application.Models.Responses
{
    public class GenericResponse<T>
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; } = [];
        public DateTime TimeStamp { get; } = DateTimeHelper.UtcNow();
        public T Data { get; set; }
    }
}
