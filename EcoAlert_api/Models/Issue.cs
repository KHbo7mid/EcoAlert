using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("issues")]
[Index("AisuggestedCategoryId", Name = "AISuggestedCategoryId")]
[Index("AssignedToId", Name = "AssignedToId")]
[Index("CategoryId", Name = "CategoryId")]
[Index("DuplicateOfIssueId", Name = "DuplicateOfIssueId")]
[Index("PriorityId", Name = "PriorityId")]
[Index("ReporterId", Name = "ReporterId")]
[Index("VerifiedById", Name = "VerifiedById")]
[Index("Latitude", "Longitude", Name = "idx_location")]
[Index("ReportedAt", Name = "idx_reported_date")]
[Index("StatusId", Name = "idx_status")]
public partial class Issue
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Description { get; set; }

    [Column("AISummary", TypeName = "text")]
    public string? Aisummary { get; set; }

    [Precision(10, 8)]
    public decimal Latitude { get; set; }

    [Precision(11, 8)]
    public decimal Longitude { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    [StringLength(100)]
    public string? City { get; set; }

    [Column(TypeName = "int(11)")]
    public int CategoryId { get; set; }

    [Column("AISuggestedCategoryId", TypeName = "int(11)")]
    public int? AisuggestedCategoryId { get; set; }

    [Column(TypeName = "int(11)")]
    public int? PriorityId { get; set; }

    [Column("AIPriorityScore")]
    [Precision(5, 2)]
    public decimal? AipriorityScore { get; set; }

    [Column(TypeName = "int(11)")]
    public int? StatusId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ReportedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VerifiedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ResolvedAt { get; set; }

    [Column(TypeName = "int(11)")]
    public int? ReporterId { get; set; }

    [Column(TypeName = "int(11)")]
    public int? AssignedToId { get; set; }

    [Column(TypeName = "int(11)")]
    public int? VerifiedById { get; set; }

    public bool? IsAnonymous { get; set; }

    [Column(TypeName = "int(11)")]
    public int? UpvoteCount { get; set; }

    [Column(TypeName = "int(11)")]
    public int? CommentCount { get; set; }

    [Column(TypeName = "int(11)")]
    public int? ViewCount { get; set; }

    [Column(TypeName = "int(11)")]
    public int? DuplicateOfIssueId { get; set; }

    [InverseProperty("Issue")]
    public virtual ICollection<Aianalysis> Aianalyses { get; set; } = new List<Aianalysis>();

    [ForeignKey("AisuggestedCategoryId")]
    [InverseProperty("IssueAisuggestedCategories")]
    public virtual Issuecategory? AisuggestedCategory { get; set; }

    [ForeignKey("AssignedToId")]
    [InverseProperty("IssueAssignedTos")]
    public virtual User? AssignedTo { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("IssueCategories")]
    public virtual Issuecategory Category { get; set; } = null!;

    [InverseProperty("Issue")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [ForeignKey("DuplicateOfIssueId")]
    [InverseProperty("InverseDuplicateOfIssue")]
    public virtual Issue? DuplicateOfIssue { get; set; }

    [InverseProperty("DuplicateOfIssue")]
    public virtual ICollection<Issue> InverseDuplicateOfIssue { get; set; } = new List<Issue>();

    [InverseProperty("Issue")]
    public virtual ICollection<Issueimage> Issueimages { get; set; } = new List<Issueimage>();

    [InverseProperty("Issue")]
    public virtual ICollection<Issueupdate> Issueupdates { get; set; } = new List<Issueupdate>();

    [InverseProperty("Issue")]
    public virtual ICollection<Issuevote> Issuevotes { get; set; } = new List<Issuevote>();

    [InverseProperty("Issue")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [ForeignKey("PriorityId")]
    [InverseProperty("Issues")]
    public virtual Issuepriority? Priority { get; set; }

    [ForeignKey("ReporterId")]
    [InverseProperty("IssueReporters")]
    public virtual User? Reporter { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Issues")]
    public virtual Issuestatus? Status { get; set; }

    [ForeignKey("VerifiedById")]
    [InverseProperty("IssueVerifiedBies")]
    public virtual User? VerifiedBy { get; set; }
}
