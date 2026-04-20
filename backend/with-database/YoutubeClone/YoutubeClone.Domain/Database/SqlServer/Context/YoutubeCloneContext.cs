using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using YoutubeClone.Domain.Database.SqlServer.Entities;

namespace YoutubeClone.Domain.Database.SqlServer.Context;

public partial class YoutubeCloneContext : DbContext
{
    public YoutubeCloneContext(DbContextOptions<YoutubeCloneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Channel> Channels { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<CreatorType> CreatorTypes { get; set; }

    public virtual DbSet<EmailTemplate> EmailTemplates { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<PlaylistVideo> PlaylistVideos { get; set; }

    public virtual DbSet<ReactionType> ReactionTypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    public virtual DbSet<UserAccountRole> UserAccountRoles { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<VideoAccessibility> VideoAccessibilities { get; set; }

    public virtual DbSet<VideoReaction> VideoReactions { get; set; }

    public virtual DbSet<ViewHistory> ViewHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(e => e.ChannelId).HasName("PK__Channel__38C3E8F4991565DB");

            entity.ToTable("Channel");

            entity.HasIndex(e => e.Handle, "UQ__Channel__FE5BB31A77770888").IsUnique();

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
            entity.HasKey(e => e.CommentId).HasName("PK__Comment__C3B4DFAABDECE649");

            entity.ToTable("Comment");

            entity.Property(e => e.CommentId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("CommentID");
            entity.Property(e => e.Content).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ParentCommentId).HasColumnName("ParentCommentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");

            entity.HasOne(d => d.ParentComment).WithMany(p => p.InverseParentComment)
                .HasForeignKey(d => d.ParentCommentId)
                .HasConstraintName("FK__Comment__ParentC__07C12930");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__UserID__05D8E0BE");

            entity.HasOne(d => d.Video).WithMany(p => p.Comments)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comment__VideoID__04E4BC85");
        });

        modelBuilder.Entity<CreatorType>(entity =>
        {
            entity.HasKey(e => e.CreatorTypeId).HasName("PK__CreatorT__2D56E80A5A323304");

            entity.ToTable("CreatorType");

            entity.HasIndex(e => e.DisplayName, "UQ__CreatorT__4E3E687D8E31AA9E").IsUnique();

            entity.Property(e => e.CreatorTypeId).HasColumnName("CreatorTypeID");
            entity.Property(e => e.DisplayName).HasMaxLength(30);
        });

        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasKey(e => e.EmailTemplateId).HasName("PK__EmailTem__BC0A387518D55A80");

            entity.HasIndex(e => e.Name, "UQ__EmailTem__737584F6DDC8FC77").IsUnique();

            entity.Property(e => e.Body).HasMaxLength(2000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Subject)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB0F7AA00ED5");

            entity.ToTable("Permission");

            entity.HasIndex(e => e.Code, "UQ__Permissi__A25C5AA78AE33E69").IsUnique();

            entity.Property(e => e.PermissionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PermissionID");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => e.PlaylistId).HasName("PK__Playlist__B30167804509690E");

            entity.ToTable("Playlist");

            entity.Property(e => e.PlaylistId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PlaylistID");
            entity.Property(e => e.ChannelId).HasColumnName("ChannelID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.CreatorTypeId).HasColumnName("CreatorTypeID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.IsPublic).HasDefaultValue(true);
            entity.Property(e => e.Title).HasMaxLength(150);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Channel).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.ChannelId)
                .HasConstraintName("FK__Playlist__Channe__114A936A");

            entity.HasOne(d => d.CreatorType).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.CreatorTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Playlist__Creato__0F624AF8");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Playlist__UserID__10566F31");
        });

        modelBuilder.Entity<PlaylistVideo>(entity =>
        {
            entity.HasKey(e => new { e.PlaylistId, e.VideoId }).HasName("PK_PlaylistVideos_PlaylistID_VideoID");

            entity.Property(e => e.PlaylistId).HasColumnName("PlaylistID");
            entity.Property(e => e.VideoId).HasColumnName("VideoID");
            entity.Property(e => e.AddedAt).HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Playlist).WithMany(p => p.PlaylistVideos)
                .HasForeignKey(d => d.PlaylistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlaylistV__Playl__19DFD96B");

            entity.HasOne(d => d.Video).WithMany(p => p.PlaylistVideos)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlaylistV__Video__1AD3FDA4");
        });

        modelBuilder.Entity<ReactionType>(entity =>
        {
            entity.HasKey(e => e.ReactionTypeId).HasName("PK__Reaction__01E625C0F9C9C90D");

            entity.ToTable("ReactionType");

            entity.HasIndex(e => e.DisplayName, "UQ__Reaction__4E3E687DCFBB724E").IsUnique();

            entity.Property(e => e.ReactionTypeId).HasColumnName("ReactionTypeID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(20);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A3D6F881A");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F60656C76C").IsUnique();

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RoleID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.RolePermissionId).HasName("PK__RolePerm__120F469AECE6C0FB");

            entity.ToTable("RolePermission");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "UQ_RolePermission").IsUnique();

            entity.Property(e => e.RolePermissionId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RolePermissionID");
            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.PermissionId).HasColumnName("PermissionID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolePermi__Permi__5BE2A6F2");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolePermi__RoleI__5AEE82B9");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.SubscriptionId).HasName("PK__Subscrip__9A2B24BDBFD69E91");

            entity.ToTable("Subscription");

            entity.HasIndex(e => new { e.UserId, e.ChannelId }, "UQ_Subscription").IsUnique();

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
                .HasConstraintName("FK__Subscript__Chann__693CA210");

            entity.HasOne(d => d.User).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Subscript__UserI__68487DD7");

            entity.HasOne(d => d.Video).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("FK__Subscript__Video__6A30C649");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId).HasName("PK__Tag__657CFA4C95300787");

            entity.ToTable("Tag");

            entity.HasIndex(e => e.DisplayName, "UQ__Tag__4E3E687D44884FA8").IsUnique();

            entity.Property(e => e.TagId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TagID");
            entity.Property(e => e.DisplayName).HasMaxLength(30);
        });

        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserAcco__1788CCAC1A3E8A6F");

            entity.ToTable("UserAccount");

            entity.HasIndex(e => e.Email, "UQ__UserAcco__A9D105346391FF1E").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__UserAcco__C9F284560D126309").IsUnique();

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(30);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.UserName).HasMaxLength(20);
        });

        modelBuilder.Entity<UserAccountRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserAcco__3D978A55F06F4FAF");

            entity.ToTable("UserAccountRole");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "UQ_UserAccountRole").IsUnique();

            entity.Property(e => e.UserRoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserRoleID");
            entity.Property(e => e.AssignedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Role).WithMany(p => p.UserAccountRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAccou__RoleI__628FA481");

            entity.HasOne(d => d.User).WithMany(p => p.UserAccountRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserAccou__UserI__619B8048");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("PK__Video__BAE5124AB3281445");

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
            entity.Property(e => e.VideoUrl)
                .HasMaxLength(500)
                .HasColumnName("VideoURL");

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
                        .HasConstraintName("FK__VideoTags__TagID__17036CC0"),
                    l => l.HasOne<Video>().WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__VideoTags__Video__160F4887"),
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
            entity.HasKey(e => e.VideoAccessibilityId).HasName("PK__VideoAcc__25970953F559463C");

            entity.ToTable("VideoAccessibility");

            entity.Property(e => e.VideoAccessibilityId).HasColumnName("VideoAccessibilityID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.DisplayName).HasMaxLength(30);
        });

        modelBuilder.Entity<VideoReaction>(entity =>
        {
            entity.HasKey(e => e.VideoReactionId).HasName("PK__VideoRea__BB33D4699B1A8446");

            entity.ToTable("VideoReaction");

            entity.HasIndex(e => new { e.VideoId, e.UserId }, "UQ_VideoReaction").IsUnique();

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
                .HasConstraintName("FK__VideoReac__React__00200768");

            entity.HasOne(d => d.User).WithMany(p => p.VideoReactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoReac__UserI__7F2BE32F");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoReactions)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__VideoReac__Video__7E37BEF6");
        });

        modelBuilder.Entity<ViewHistory>(entity =>
        {
            entity.HasKey(e => e.ViewHistoryId).HasName("PK__ViewHist__55D4BB136485B5E6");

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
                .HasConstraintName("FK__ViewHisto__UserI__6EF57B66");

            entity.HasOne(d => d.Video).WithMany(p => p.ViewHistories)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ViewHisto__Video__6FE99F9F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
