namespace YoutubeClone.Application.Models.Requests.Channels
{
    public class GetAllChannelRequest
    {
        public int? Limit { get; set; }
        string patronPassword = @"^(?=.*[A-Z])(?=.*[\W_]).+$";
        public int? Offset { get; set; }
    }
}
