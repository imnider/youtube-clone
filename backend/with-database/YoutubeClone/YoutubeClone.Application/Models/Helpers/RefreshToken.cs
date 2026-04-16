namespace YoutubeClone.Application.Models.Helpers
{
    public class RefreshToken
    {
        public required Guid CollaboratorId { get; set; }
        public required TimeSpan ExpirationInDays { get; set; }
    }
}
