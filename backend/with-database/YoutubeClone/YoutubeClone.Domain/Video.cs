using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class Video
{
    public Guid VideoId { get; set; }

    public Guid ChannelId { get; set; }

    public int VideoAccessibilityId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int DurationSeconds { get; set; }

    public string ThumbnailUrl { get; set; } = null!;

    public bool AgeRestriction { get; set; }

    public DateTime PublishedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual VideoAccessibility VideoAccessibility { get; set; } = null!;

    public virtual ICollection<VideoReaction> VideoReactions { get; set; } = new List<VideoReaction>();

    public virtual ICollection<ViewHistory> ViewHistories { get; set; } = new List<ViewHistory>();

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
