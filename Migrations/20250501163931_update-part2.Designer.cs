﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuanLyBaiBaoKhoaHoc.Models;

#nullable disable

namespace QuanLyBaiBaoKhoaHoc.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250501163931_update-part2")]
    partial class updatepart2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ArticleTopic", b =>
                {
                    b.Property<int>("ArticleId")
                        .HasColumnType("int");

                    b.Property<int>("TopicId")
                        .HasColumnType("int");

                    b.HasKey("ArticleId", "TopicId");

                    b.HasIndex("TopicId");

                    b.ToTable("ArticleTopics");
                });

            modelBuilder.Entity("QuanLyBaiBaoKhoaHoc.Models.Article", b =>
                {
                    b.Property<int>("ArticleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArticleId"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmittedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArticleId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("QuanLyBaiBaoKhoaHoc.Models.Topic", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TopicId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TopicId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("QuanLyBaiBaoKhoaHoc.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            DateOfBirth = new DateTime(2005, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "doquockien12345@gmail.com",
                            FullName = "Admin",
                            PasswordHash = "admin",
                            Role = 1,
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("ArticleTopic", b =>
                {
                    b.HasOne("QuanLyBaiBaoKhoaHoc.Models.Article", "Article")
                        .WithMany("ArticleTopics")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuanLyBaiBaoKhoaHoc.Models.Topic", "Topic")
                        .WithMany("ArticleTopics")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("QuanLyBaiBaoKhoaHoc.Models.Article", b =>
                {
                    b.HasOne("QuanLyBaiBaoKhoaHoc.Models.User", "Author")
                        .WithMany("Articles")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("QuanLyBaiBaoKhoaHoc.Models.Article", b =>
                {
                    b.Navigation("ArticleTopics");
                });

            modelBuilder.Entity("QuanLyBaiBaoKhoaHoc.Models.Topic", b =>
                {
                    b.Navigation("ArticleTopics");
                });

            modelBuilder.Entity("QuanLyBaiBaoKhoaHoc.Models.User", b =>
                {
                    b.Navigation("Articles");
                });
#pragma warning restore 612, 618
        }
    }
}
