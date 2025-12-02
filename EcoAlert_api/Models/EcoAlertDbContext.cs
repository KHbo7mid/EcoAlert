using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace EcoAlert.Models;

public partial class EcoAlertDbContext : DbContext
{
    public EcoAlertDbContext()
    {
    }

    public EcoAlertDbContext(DbContextOptions<EcoAlertDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aianalysis> Aianalyses { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Issue> Issues { get; set; }

    public virtual DbSet<Issuecategory> Issuecategories { get; set; }

    public virtual DbSet<Issueimage> Issueimages { get; set; }

    public virtual DbSet<Issuepriority> Issuepriorities { get; set; }

    public virtual DbSet<Issuestatus> Issuestatuses { get; set; }

    public virtual DbSet<Issueupdate> Issueupdates { get; set; }

    public virtual DbSet<Issuevote> Issuevotes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    public virtual DbSet<Votetype> Votetypes { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Aianalysis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.AnalyzedAt).HasDefaultValueSql("current_timestamp()");

            entity.HasOne(d => d.Image).WithMany(p => p.Aianalyses)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("aianalyses_ibfk_2");

            entity.HasOne(d => d.Issue).WithMany(p => p.Aianalyses).HasConstraintName("aianalyses_ibfk_1");

            entity.HasOne(d => d.SuggestedCategory).WithMany(p => p.Aianalyses).HasConstraintName("aianalyses_ibfk_3");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("current_timestamp()");
            entity.Property(e => e.IsOfficialUpdate).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.Issue).WithMany(p => p.Comments).HasConstraintName("comments_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Comments).HasConstraintName("comments_ibfk_2");
        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.AipriorityScore).HasDefaultValueSql("'0.00'");
            entity.Property(e => e.CommentCount).HasDefaultValueSql("'0'");
            entity.Property(e => e.IsAnonymous).HasDefaultValueSql("'0'");
            entity.Property(e => e.PriorityId).HasDefaultValueSql("'1'");
            entity.Property(e => e.ReportedAt).HasDefaultValueSql("current_timestamp()");
            entity.Property(e => e.StatusId).HasDefaultValueSql("'1'");
            entity.Property(e => e.UpvoteCount).HasDefaultValueSql("'0'");
            entity.Property(e => e.ViewCount).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.AisuggestedCategory).WithMany(p => p.IssueAisuggestedCategories).HasConstraintName("issues_ibfk_2");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.IssueAssignedTos).HasConstraintName("issues_ibfk_6");

            entity.HasOne(d => d.Category).WithMany(p => p.IssueCategories)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("issues_ibfk_1");

            entity.HasOne(d => d.DuplicateOfIssue).WithMany(p => p.InverseDuplicateOfIssue).HasConstraintName("issues_ibfk_8");

            entity.HasOne(d => d.Priority).WithMany(p => p.Issues).HasConstraintName("issues_ibfk_3");

            entity.HasOne(d => d.Reporter).WithMany(p => p.IssueReporters).HasConstraintName("issues_ibfk_5");

            entity.HasOne(d => d.Status).WithMany(p => p.Issues).HasConstraintName("issues_ibfk_4");

            entity.HasOne(d => d.VerifiedBy).WithMany(p => p.IssueVerifiedBies).HasConstraintName("issues_ibfk_7");
        });

        modelBuilder.Entity<Issuecategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.PriorityWeight).HasDefaultValueSql("'1.00'");
        });

        modelBuilder.Entity<Issueimage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.IsPrimary).HasDefaultValueSql("'0'");
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("current_timestamp()");

            entity.HasOne(d => d.Issue).WithMany(p => p.Issueimages).HasConstraintName("issueimages_ibfk_1");
        });

        modelBuilder.Entity<Issuepriority>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Issuestatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Issueupdate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("current_timestamp()");

            entity.HasOne(d => d.Issue).WithMany(p => p.Issueupdates).HasConstraintName("issueupdates_ibfk_1");

            entity.HasOne(d => d.NewStatus).WithMany(p => p.IssueupdateNewStatuses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("issueupdates_ibfk_3");

            entity.HasOne(d => d.OldStatus).WithMany(p => p.IssueupdateOldStatuses).HasConstraintName("issueupdates_ibfk_2");

            entity.HasOne(d => d.UpdatedBy).WithMany(p => p.Issueupdates).HasConstraintName("issueupdates_ibfk_4");
        });

        modelBuilder.Entity<Issuevote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.VotedAt).HasDefaultValueSql("current_timestamp()");

            entity.HasOne(d => d.Issue).WithMany(p => p.Issuevotes).HasConstraintName("issuevotes_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Issuevotes).HasConstraintName("issuevotes_ibfk_1");

            entity.HasOne(d => d.VoteType).WithMany(p => p.Issuevotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("issuevotes_ibfk_3");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("current_timestamp()");
            entity.Property(e => e.IsRead).HasDefaultValueSql("'0'");

            entity.HasOne(d => d.Issue).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("notifications_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications).HasConstraintName("notifications_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("current_timestamp()");
            entity.Property(e => e.IsActive).HasDefaultValueSql("'1'");
            entity.Property(e => e.RoleId).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_ibfk_1");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        modelBuilder.Entity<Votetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
