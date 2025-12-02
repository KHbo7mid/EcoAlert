using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("issueupdates")]
[Index("NewStatusId", Name = "NewStatusId")]
[Index("OldStatusId", Name = "OldStatusId")]
[Index("UpdatedById", Name = "UpdatedById")]
[Index("IssueId", Name = "idx_issue_updates")]
public partial class Issueupdate
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [Column(TypeName = "int(11)")]
    public int IssueId { get; set; }

    [Column(TypeName = "int(11)")]
    public int? OldStatusId { get; set; }

    [Column(TypeName = "int(11)")]
    public int NewStatusId { get; set; }

    [Column(TypeName = "text")]
    public string Description { get; set; } = null!;

    [Column(TypeName = "int(11)")]
    public int? UpdatedById { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("Issueupdates")]
    public virtual Issue Issue { get; set; } = null!;

    [ForeignKey("NewStatusId")]
    [InverseProperty("IssueupdateNewStatuses")]
    public virtual Issuestatus NewStatus { get; set; } = null!;

    [ForeignKey("OldStatusId")]
    [InverseProperty("IssueupdateOldStatuses")]
    public virtual Issuestatus? OldStatus { get; set; }

    [ForeignKey("UpdatedById")]
    [InverseProperty("Issueupdates")]
    public virtual User? UpdatedBy { get; set; }
}
