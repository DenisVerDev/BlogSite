using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Models;

public partial class BlogSiteContext : DbContext
{
    public BlogSiteContext()
    {
    }

    public BlogSiteContext(DbContextOptions<BlogSiteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=localhost;Database=BlogSite;Trusted_Connection=True;Encrypt=false;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => new { e.Title, e.Author }).HasName("PK_Post");

            entity.HasIndex(e => e.PostId, "UK_PostId").IsUnique();

            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.Author).HasMaxLength(100);
            entity.Property(e => e.Content).HasMaxLength(1000);
            entity.Property(e => e.CreationDate).HasColumnType("date");
            entity.Property(e => e.LastUpdateDate).HasColumnType("date");
            entity.Property(e => e.PostId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.PostsNavigation)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("FK_PostAuthor");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Username).HasName("PK_User");

            entity.HasIndex(e => e.Email, "UK_UserEmail").IsUnique();

            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasMany(d => d.Authors).WithMany(p => p.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "FollowersAuthors",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Author")
                        .HasConstraintName("FK_FAAuthor"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Follower")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FAFollower"),
                    j =>
                    {
                        j.HasKey("Follower", "Author");
                        j.ToTable("FollowersAuthors");
                        j.IndexerProperty<string>("Follower").HasMaxLength(100);
                        j.IndexerProperty<string>("Author").HasMaxLength(100);
                    });

            entity.HasMany(d => d.Followers).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "FollowersAuthors",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("Follower")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_FAFollower"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Author")
                        .HasConstraintName("FK_FAAuthor"),
                    j =>
                    {
                        j.HasKey("Follower", "Author");
                        j.ToTable("FollowersAuthors");
                        j.IndexerProperty<string>("Follower").HasMaxLength(100);
                        j.IndexerProperty<string>("Author").HasMaxLength(100);
                    });

            entity.HasMany(d => d.Posts).WithMany(p => p.Likers)
                .UsingEntity<Dictionary<string, object>>(
                    "LikedPosts",
                    r => r.HasOne<Post>().WithMany()
                        .HasPrincipalKey("PostId")
                        .HasForeignKey("PostId")
                        .HasConstraintName("FK_LPPost"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Liker")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_LPUser"),
                    j =>
                    {
                        j.HasKey("Liker", "PostId").HasName("PK_LikedPost");
                        j.ToTable("LikedPosts");
                        j.IndexerProperty<string>("Liker").HasMaxLength(100);
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
