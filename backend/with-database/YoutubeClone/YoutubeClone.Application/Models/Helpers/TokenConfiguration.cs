using Microsoft.IdentityModel.Tokens;

namespace YoutubeClone.Application.Models.Helpers
{
    public class TokenConfiguration
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required SymmetricSecurityKey SecurityKey { get; set; }
        public required DateTime ExpirationDateTime { get; set; }
        public required TimeSpan ExpirationTimeSpan { get; set; }
    }
}
