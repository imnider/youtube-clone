using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class Playlist
{
    public Guid PlaylistId { get; set; }

    public int CreatorTypeId { get; set; }

    public Guid UserId { get; set; }

    public Guid? ChannelId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Channel? Channel { get; set; }

    public virtual CreatorType CreatorType { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
