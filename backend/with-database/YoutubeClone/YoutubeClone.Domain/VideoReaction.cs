using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class VideoReaction
{
    public Guid VideoReactionId { get; set; }

    public Guid VideoId { get; set; }

    public Guid UserId { get; set; }

    public int ReactionTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ReactionType ReactionType { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
