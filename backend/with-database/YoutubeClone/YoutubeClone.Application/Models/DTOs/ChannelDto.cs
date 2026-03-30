namespace YoutubeClone.Application.Models.DTOs
{
    public class ChannelDto
    {
        public Guid ChannelId { get; set; }
        public Guid UserId { get; set; }
        public string Handle { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public bool Verification { get; set; }
        public string? Description { get; set; }
        public string? AvatarURL { get; set; }
        public string? BannerURL { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
