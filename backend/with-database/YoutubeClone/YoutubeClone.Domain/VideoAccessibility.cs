using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class VideoAccessibility
{
    public int VideoAccessibilityId { get; set; }

    public string DisplayName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
