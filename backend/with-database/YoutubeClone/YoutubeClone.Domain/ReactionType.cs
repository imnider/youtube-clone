using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class ReactionType
{
    public int ReactionTypeId { get; set; }

    public string DisplayName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<VideoReaction> VideoReactions { get; set; } = new List<VideoReaction>();
}
