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

    public virtual DbSet<Theme> Themes { get; set; }

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

            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.CreationDate).HasColumnType("date");
            entity.Property(e => e.LastUpdateDate).HasColumnType("date");
            entity.Property(e => e.PostId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.Author)
                .HasConstraintName("FK_PostAuthor");

            entity.HasOne(d => d.ThemeNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.Theme)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PostTheme");
        });

        modelBuilder.Entity<Theme>(entity =>
        {
            entity.HasKey(e => e.ThemeId).HasName("PK_Theme");

            entity.HasIndex(e => e.Name, "UK_ThemeName").IsUnique();

            entity.Property(e => e.ThemeId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_User");

            entity.HasIndex(e => e.Email, "UK_UserEmail").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(20);

            entity.HasMany(d => d.Authors).WithMany(p => p.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "FollowersAuthor",
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
                    });

            entity.HasMany(d => d.Followers).WithMany(p => p.Authors)
                .UsingEntity<Dictionary<string, object>>(
                    "FollowersAuthor",
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
                    });

            entity.HasMany(d => d.LikedPosts).WithMany(p => p.Likers)
                .UsingEntity<Dictionary<string, object>>(
                    "LikedPost",
                    r => r.HasOne<Post>().WithMany()
                        .HasPrincipalKey("PostId")
                        .HasForeignKey("LikedPost1")
                        .HasConstraintName("FK_LPPost"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Liker")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_LPUser"),
                    j =>
                    {
                        j.HasKey("Liker", "LikedPost1").HasName("PK_LikedPost");
                        j.ToTable("LikedPosts");
                        j.IndexerProperty<int>("LikedPost1").HasColumnName("LikedPost");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
