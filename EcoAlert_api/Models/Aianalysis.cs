using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("aianalyses")]
[Index("ImageId", Name = "ImageId")]
[Index("SuggestedCategoryId", Name = "SuggestedCategoryId")]
[Index("IssueId", Name = "idx_issue_analysis")]
public partial class Aianalysis
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [Column(TypeName = "int(11)")]
    public int IssueId { get; set; }

    [Column(TypeName = "int(11)")]
    public int? ImageId { get; set; }

    [Column(TypeName = "json")]
    public string? DetectedObjects { get; set; }

    [Column(TypeName = "int(11)")]
    public int? SuggestedCategoryId { get; set; }

    [Precision(5, 2)]
    public decimal? UrgencyScore { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? AnalyzedAt { get; set; }

    [ForeignKey("ImageId")]
    [InverseProperty("Aianalyses")]
    public virtual Issueimage? Image { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("Aianalyses")]
    public virtual Issue Issue { get; set; } = null!;

    [ForeignKey("SuggestedCategoryId")]
    [InverseProperty("Aianalyses")]
    public virtual Issuecategory? SuggestedCategory { get; set; }
}
