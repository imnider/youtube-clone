using System.ComponentModel.DataAnnotations;

namespace YoutubeClone.Application.Models.Requests.Auth
{
    public class RenewAuthRequest
    {
        [Required]
        public string RefreshToken { get; set; } = null!;
    }
}
