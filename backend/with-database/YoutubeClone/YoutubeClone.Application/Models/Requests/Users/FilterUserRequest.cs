namespace YoutubeClone.Application.Models.Requests.Users
{
    public class FilterUserRequest : BaseRequest
    {
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Location { get; set; }
    }
}
