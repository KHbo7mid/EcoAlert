using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("issueimages")]
[Index("IssueId", Name = "idx_issue")]
public partial class Issueimage
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [Column(TypeName = "int(11)")]
    public int IssueId { get; set; }

    [StringLength(500)]
    public string ImageUrl { get; set; } = null!;

    [StringLength(500)]
    public string? ThumbnailUrl { get; set; }

    [Column("AITags", TypeName = "json")]
    public string? Aitags { get; set; }

    public bool? IsPrimary { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UploadedAt { get; set; }

    [InverseProperty("Image")]
    public virtual ICollection<Aianalysis> Aianalyses { get; set; } = new List<Aianalysis>();

    [ForeignKey("IssueId")]
    [InverseProperty("Issueimages")]
    public virtual Issue Issue { get; set; } = null!;
}
