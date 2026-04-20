using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain.Database.SqlServer.Entities;

public partial class PlaylistVideo
{
    public Guid PlaylistId { get; set; }

    public Guid VideoId { get; set; }

    public DateTime AddedAt { get; set; }

    public virtual Playlist Playlist { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
