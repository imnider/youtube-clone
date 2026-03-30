using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class Channel
{
    public Guid ChannelId { get; set; }

    public Guid UserId { get; set; }

    public string Handle { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public bool Verification { get; set; }

    public string? Description { get; set; }

    public string? AvatarUrl { get; set; }

    public string? BannerUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual UserAccount User { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
