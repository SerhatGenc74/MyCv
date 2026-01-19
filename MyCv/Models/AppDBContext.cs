using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyCv.Models;

public partial class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminUser> AdminUsers { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<Portfolio> Portfolios { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectDetail> ProjectDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-QGPTUQ6\\SQLEXPRESS,1433;Initial Catalog=CVWebSiteDB;User ID=testuser;Password=1234;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__AdminUse__CB9A1CFFC22BE178");

            entity.HasIndex(e => new { e.UserId, e.Role }, "AUIX1");

            entity.HasIndex(e => new { e.UserId, e.NickName, e.Role }, "AUIX2");

            entity.HasIndex(e => e.NickName, "UQ__AdminUse__48F06EC157F4498D").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__AdminUse__AB6E6164CEA77A87").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(5)
                .IsFixedLength()
                .HasColumnName("userId");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.NickName)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("nickName");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("PK__Contents__0BDC8719637FE0CA");

            entity.HasIndex(e => e.Type, "CIX1");

            entity.Property(e => e.ContentId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("contentId");
            entity.Property(e => e.Content1)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.DeleteId).HasColumnName("deleteId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.SubContent)
                .IsUnicode(false)
                .HasColumnName("subContent");
            entity.Property(e => e.Tags)
                .IsUnicode(false)
                .HasColumnName("tags");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("type");
            entity.Property(e => e.VisibleContent)
                .IsUnicode(false)
                .HasColumnName("visibleContent");
        });

        modelBuilder.Entity<Portfolio>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Portfolio");

            entity.HasIndex(e => e.Tag, "UQ__Portfoli__DC101C019C4C3F27").IsUnique();

            entity.Property(e => e.CoverImgUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("coverImgUrl");
            entity.Property(e => e.DeleteId).HasColumnName("deleteId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Tag)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("tag");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProjectId).HasName("PK__Projects__11F14DA5D792E108");

            entity.HasIndex(e => new { e.ProjectId, e.DeleteId }, "PIX1");

            entity.Property(e => e.ProjectId)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("projectId");
            entity.Property(e => e.CoverImgUrl)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("coverImgUrl");
            entity.Property(e => e.DeleteId).HasColumnName("deleteId");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Tags)
                .IsUnicode(false)
                .HasColumnName("tags");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<ProjectDetail>(entity =>
        {
            entity.HasNoKey();

            entity.HasIndex(e => new { e.Id, e.ProjectId }, "PDIX1");

            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.DeleteId).HasColumnName("deleteId");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.ProjectId)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("projectId");
            entity.Property(e => e.SubContent)
                .IsUnicode(false)
                .HasColumnName("subContent");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("type");
            entity.Property(e => e.VisibleContent)
                .IsUnicode(false)
                .HasColumnName("visibleContent");

            entity.HasOne(d => d.Project).WithMany()
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectDe__proje__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
