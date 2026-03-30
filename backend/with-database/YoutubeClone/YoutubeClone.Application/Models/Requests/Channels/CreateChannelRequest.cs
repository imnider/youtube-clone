using System.ComponentModel.DataAnnotations;
using YoutubeClone.Shared.Constants;

namespace YoutubeClone.Application.Models.Requests.Channels
{
    public class CreateChannelRequest
    {
        [Required(ErrorMessage = ValidationConstants.REQUIRED)]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = ValidationConstants.REQUIRED)]
        [MaxLength(30, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        [MinLength(5, ErrorMessage = ValidationConstants.MIN_LENGTH)]
        public string Handle { get; set; } = null!;

        [Required(ErrorMessage = ValidationConstants.REQUIRED)]
        [MaxLength(50, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        [MinLength(10, ErrorMessage = ValidationConstants.MIN_LENGTH)]
        public string DisplayName { get; set; } = null!;

        [MaxLength(255, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        public string? Description { get; set; }

        [MaxLength(255, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        public string? AvatarUrl { get; set; }

        [MaxLength(255, ErrorMessage = ValidationConstants.MAX_LENGTH)]
        public string? BannerUrl { get; set; }
    }
}
