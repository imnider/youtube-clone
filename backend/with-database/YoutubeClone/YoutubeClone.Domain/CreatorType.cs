using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class CreatorType
{
    public int CreatorTypeId { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();
}
