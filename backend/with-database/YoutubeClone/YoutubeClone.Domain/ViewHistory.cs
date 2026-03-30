using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain;

public partial class ViewHistory
{
    public Guid ViewHistoryId { get; set; }

    public Guid UserId { get; set; }

    public Guid VideoId { get; set; }

    public decimal CompletionRate { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual UserAccount User { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
