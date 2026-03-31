namespace YoutubeClone.Application.Models.Requests
{
    public class BaseRequest
    {
        public int Limit { get; set; } = 100;
        public int Offset { get; set; } = 0;
    }
}
