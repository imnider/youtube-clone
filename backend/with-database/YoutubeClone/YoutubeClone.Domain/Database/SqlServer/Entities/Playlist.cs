using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain.Database.SqlServer.Entities;

public partial class Playlist
{
    public Guid PlaylistId { get; set; }

    public int CreatorTypeId { get; set; }

    public Guid UserId { get; set; }

    public Guid? ChannelId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsPublic { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Channel? Channel { get; set; }

    public virtual CreatorType CreatorType { get; set; } = null!;

    public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; } = new List<PlaylistVideo>();

    public virtual UserAccount User { get; set; } = null!;
}
