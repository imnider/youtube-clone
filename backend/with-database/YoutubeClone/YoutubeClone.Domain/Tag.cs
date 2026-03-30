using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class Tag
{
    public Guid TagId { get; set; }

    public string DisplayName { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
