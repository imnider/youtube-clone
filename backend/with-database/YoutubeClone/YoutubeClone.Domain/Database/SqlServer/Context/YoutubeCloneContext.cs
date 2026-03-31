using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YoutubeClone.Domain.Database.SqlServer.Entities;

namespace YoutubeClone.Domain.Database.SqlServer.Context;

public partial class YoutubeCloneContext : DbContext
{
    public YoutubeCloneContext()
    {
    }

    public YoutubeCloneContext(DbContextOptions<YoutubeCloneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CreatorType> CreatorTypes { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<ReactionType> ReactionTypes { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<VideoAccessibility> VideoAccessibilities { get; set; }

    public virtual DbSet<VideoReaction> VideoReactions { get; set; }

    public virtual DbSet<ViewHistory> ViewHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;User=sa;Password=Admin1234@;Database=YoutubeClone;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.ChannelId).HasName("PK__Channel__38C3E8F468DB71A5");

            entity.ToTable("Channel");

            entity.HasIndex(e => e.Handle, "UQ__Channel__FE5BB31A2BE128EC").IsUnique();

            entity.Property(e => e.ChannelId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ChannelID");
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(255)
                .HasColumnName("AvatarURL");
            entity.Property(e => e.BannerUrl)
                .HasMaxLength(255)
                .HasColumnName("BannerURL");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.Handle).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Channels)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Channel__UserID__3F466844");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAA7CECA9B7");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("CommentID");
            entity.Property(e => e.Content).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ParentCommentId).HasColumnName("ParentCommentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__Comment__ParentC__6C190EBB");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__UserID__6A30C649");

            entity.HasOne(d => d.Video).WithMany(p => p.Comments)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__VideoID__693CA210");
        });

        modelBuilder.Entity<CreatorType>(entity =>
        {
            entity.HasKey(e => e.CreatorTypeId).HasName("PK__CreatorT__2D56E80AF0EC9BC6");

            entity.ToTable("CreatorType");

            entity.Property(e => e.CreatorTypeId).HasColumnName("CreatorTypeID");
            entity.Property(e => e.DisplayName).HasMaxLength(30);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("PK__Playlist__B3016780903BAE49");

            entity.ToTable("Playlist");

            entity.Property(e => e.PlaylistId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PlaylistID");
            entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatorTypeId).HasColumnName("CreatorTypeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Channel).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.ChannelId)
                .HasConstraintName("FK__Playlist__Channe__74AE54BC");

            entity.HasOne(d => d.CreatorType).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.CreatorTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Playlist__Creato__72C60C4A");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Playlist__UserID__73BA3083");

            entity.HasMany(d => d.Videos).WithMany(p => p.Playlists)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaylistVideo",
                    r => r.HasOne<Video>().WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PlaylistV__Video__7D439ABD"),
                    l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__PlaylistV__Playl__7C4F7684"),
                    j =>
                    {
                        j.HasKey("PlaylistId", "VideoId").HasName("PK_PlaylistVideos_PlaylistID_VideoID");
                        j.ToTable("PlaylistVideos");
                        j.IndexerProperty<Guid>("PlaylistId").HasColumnName("PlaylistID");
                        j.IndexerProperty<Guid>("VideoId").HasColumnName("VideoID");
                    });
        });

        modelBuilder.Entity<ReactionType>(entity =>
        {
            entity.HasKey(e => e.ReactionTypeId).HasName("PK__Reaction__01E625C0A8171281");

            entity.ToTable("ReactionType");

            entity.Property(e => e.ReactionTypeId).HasColumnName("ReactionTypeID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(20);
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B24BDB8943D02");

            entity.ToTable("Subscription");

            entity.Property(e => e.SubscriptionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("SubscriptionID");
            entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");

            entity.HasOne(d => d.Channel).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__Chann__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__UserI__4F7CD00D");

            entity.HasOne(d => d.Video).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("FK__Subscript__Video__5165187F");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tag__657CFA4CDAE8D016");

            entity.ToTable("Tag");

            entity.Property(e => e.TagId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TagID");
            entity.Property(e => e.DisplayName).HasMaxLength(20);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CCACAC39A20D");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Email, "UQ__UserAcco__A9D10534CA935AD7").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__UserAcco__C9F28456F878FB0C").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(40);
            entity.Property(e => e.UserName).HasMaxLength(20);
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__Video__BAE5124A74C9D433");

            entity.ToTable("Video");

            entity.Property(e => e.VideoId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("VideoID");
            entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.PublishedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(255)
                .HasColumnName("ThumbnailURL");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.VideoAccessibilityId).HasColumnName("VideoAccessibilityID");

            entity.HasOne(d => d.Channel).WithMany(p => p.Videos)
                .HasForeignKey(d => d.ChannelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Video__ChannelID__47DBAE45");

            entity.HasOne(d => d.VideoAccessibility).WithMany(p => p.Videos)
                .HasForeignKey(d => d.VideoAccessibilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Video__VideoAcce__48CFD27E");

            entity.HasMany(d => d.Tags).WithMany(p => p.Videos)
                .UsingEntity<Dictionary<string, object>>(
                    "VideoTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__VideoTags__TagID__797309D9"),
                    l => l.HasOne<Video>().WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__VideoTags__Video__787EE5A0"),
                    j =>
                    {
                        j.HasKey("VideoId", "TagId").HasName("PK_VideoTags_VideoID_TagID");
                        j.ToTable("VideoTags");
                        j.IndexerProperty<Guid>("VideoId").HasColumnName("VideoID");
                        j.IndexerProperty<Guid>("TagId").HasColumnName("TagID");
                    });
        });

        modelBuilder.Entity<VideoAccessibility>(entity =>
        {
            entity.HasKey(e => e.VideoAccessibilityId).HasName("PK__VideoAcc__25970953B314D0A4");

            entity.ToTable("VideoAccessibility");

            entity.Property(e => e.VideoAccessibilityId).HasColumnName("VideoAccessibilityID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(30);
        });

        modelBuilder.Entity<VideoReaction>(entity =>
        {
            entity.HasKey(e => e.VideoReactionId).HasName("PK__VideoRea__BB33D469A45A31EB");

            entity.ToTable("VideoReaction");

            entity.Property(e => e.VideoReactionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("VideoReactionID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ReactionTypeId).HasColumnName("ReactionTypeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");

            entity.HasOne(d => d.ReactionType).WithMany(p => p.VideoReactions)
                .HasForeignKey(d => d.ReactionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoReac__React__6477ECF3");

            entity.HasOne(d => d.User).WithMany(p => p.VideoReactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoReac__UserI__6383C8BA");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoReactions)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoReac__Video__628FA481");
        });

        modelBuilder.Entity<ViewHistory>(entity =>
        {
            entity.HasKey(e => e.ViewHistoryId).HasName("PK__ViewHist__55D4BB138AE1D33C");

            entity.ToTable("ViewHistory");

            entity.Property(e => e.ViewHistoryId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ViewHistoryID");
            entity.Property(e => e.CompletionRate).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");

            entity.HasOne(d => d.User).WithMany(p => p.ViewHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ViewHisto__UserI__5629CD9C");

            entity.HasOne(d => d.Video).WithMany(p => p.ViewHistories)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ViewHisto__Video__571DF1D5");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
