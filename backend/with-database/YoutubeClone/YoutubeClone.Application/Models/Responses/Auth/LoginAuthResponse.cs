namespace YoutubeClone.Application.Models.Responses.Auth
{
    public class LoginAuthResponse
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
    }
}
