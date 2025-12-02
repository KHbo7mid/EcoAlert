using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("issuevotes")]
[Index("VoteTypeId", Name = "VoteTypeId")]
[Index("IssueId", Name = "idx_issue_votes")]
[Index("UserId", "IssueId", Name = "unique_user_issue", IsUnique = true)]
public partial class Issuevote
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [Column(TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column(TypeName = "int(11)")]
    public int IssueId { get; set; }

    [Column(TypeName = "int(11)")]
    public int VoteTypeId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? VotedAt { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("Issuevotes")]
    public virtual Issue Issue { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Issuevotes")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("VoteTypeId")]
    [InverseProperty("Issuevotes")]
    public virtual Votetype VoteType { get; set; } = null!;
}
