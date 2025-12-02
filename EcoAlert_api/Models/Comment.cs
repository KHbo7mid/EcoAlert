using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("comments")]
[Index("UserId", Name = "UserId")]
[Index("IssueId", Name = "idx_issue_comments")]
public partial class Comment
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [Column(TypeName = "int(11)")]
    public int IssueId { get; set; }

    [Column(TypeName = "int(11)")]
    public int UserId { get; set; }

    [Column(TypeName = "text")]
    public string Content { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    public bool? IsOfficialUpdate { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("Comments")]
    public virtual Issue Issue { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Comments")]
    public virtual User User { get; set; } = null!;
}
