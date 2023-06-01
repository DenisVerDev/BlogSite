﻿// <auto-generated />
using System;
using BlogSite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlogSite.Migrations
{
    [DbContext(typeof(BlogSiteContext))]
    [Migration("20230601085010_RoleUpdate")]
    partial class RoleUpdate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BlogSite.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<int>("Author")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("date");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("date");

                    b.Property<int>("Theme")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("PostId")
                        .HasName("PK_Post");

                    b.HasIndex("Author");

                    b.HasIndex("Theme");

                    b.HasIndex(new[] { "Title", "Author" }, "UK_PostTitleAuthor")
                        .IsUnique();

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BlogSite.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("RoleId")
                        .HasName("PK_Role");

                    b.HasIndex(new[] { "RoleName" }, "UK_RoleName")
                        .IsUnique();

                    b.ToTable("Role");
                });

            modelBuilder.Entity("BlogSite.Models.Theme", b =>
                {
                    b.Property<int>("ThemeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ThemeId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<byte[]>("ThemeImage")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("ThemeId")
                        .HasName("PK_Theme");

                    b.HasIndex(new[] { "Name" }, "UK_ThemeName")
                        .IsUnique();

                    b.ToTable("Themes");
                });

            modelBuilder.Entity("BlogSite.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("UserId")
                        .HasName("PK_User");

                    b.HasIndex(new[] { "Email" }, "UK_UserEmail")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FollowersAuthor", b =>
                {
                    b.Property<int>("Follower")
                        .HasColumnType("int");

                    b.Property<int>("Author")
                        .HasColumnType("int");

                    b.HasKey("Follower", "Author");

                    b.HasIndex("Author");

                    b.ToTable("FollowersAuthors", (string)null);
                });

            modelBuilder.Entity("LikedPost", b =>
                {
                    b.Property<int>("Liker")
                        .HasColumnType("int");

                    b.Property<int>("LikedPost1")
                        .HasColumnType("int")
                        .HasColumnName("LikedPost");

                    b.HasKey("Liker", "LikedPost1")
                        .HasName("PK_LikedPost");

                    b.HasIndex("LikedPost1");

                    b.ToTable("LikedPosts", (string)null);
                });

            modelBuilder.Entity("BlogSite.Models.Post", b =>
                {
                    b.HasOne("BlogSite.Models.User", "AuthorNavigation")
                        .WithMany("Posts")
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_PostAuthor");

                    b.HasOne("BlogSite.Models.Theme", "ThemeNavigation")
                        .WithMany("Posts")
                        .HasForeignKey("Theme")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("FK_PostTheme");

                    b.Navigation("AuthorNavigation");

                    b.Navigation("ThemeNavigation");
                });

            modelBuilder.Entity("FollowersAuthor", b =>
                {
                    b.HasOne("BlogSite.Models.User", null)
                        .WithMany()
                        .HasForeignKey("Author")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_FAAuthor");

                    b.HasOne("BlogSite.Models.User", null)
                        .WithMany()
                        .HasForeignKey("Follower")
                        .IsRequired()
                        .HasConstraintName("FK_FAFollower");
                });

            modelBuilder.Entity("LikedPost", b =>
                {
                    b.HasOne("BlogSite.Models.Post", null)
                        .WithMany()
                        .HasForeignKey("LikedPost1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_LPPost");

                    b.HasOne("BlogSite.Models.User", null)
                        .WithMany()
                        .HasForeignKey("Liker")
                        .IsRequired()
                        .HasConstraintName("FK_LPUser");
                });

            modelBuilder.Entity("BlogSite.Models.Theme", b =>
                {
                    b.Navigation("Posts");
                });

            modelBuilder.Entity("BlogSite.Models.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}