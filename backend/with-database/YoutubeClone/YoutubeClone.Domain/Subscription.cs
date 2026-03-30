using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class Subscription
{
    public Guid SubscriptionId { get; set; }

    public Guid UserId { get; set; }

    public Guid ChannelId { get; set; }

    public Guid? VideoId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Channel Channel { get; set; } = null!;

    public virtual UserAccount User { get; set; } = null!;

    public virtual Video? Video { get; set; }
}
