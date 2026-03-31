using System;
using System.Collections.Generic;

namespace YoutubeClone.Domain.Database.SqlServer.Entities;

public partial class Comment
{
    public Guid CommentId { get; set; }

    public Guid VideoId { get; set; }

    public Guid UserId { get; set; }

    public string Content { get; set; } = null!;

    public bool IsPinned { get; set; }

    public Guid? ParentCommentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Comment> InverseParentComment { get; set; } = new List<Comment>();

    public virtual Comment? ParentComment { get; set; }

    public virtual UserAccount User { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
