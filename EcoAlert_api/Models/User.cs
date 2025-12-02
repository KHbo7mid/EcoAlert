using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("users")]
[Index("Email", Name = "Email", IsUnique = true)]
[Index("RoleId", Name = "RoleId")]
public partial class User
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string HashedPassword { get; set; } = null!;

    [StringLength(100)]
    public string? DisplayName { get; set; }

    [Column(TypeName = "int(11)")]
    public int RoleId { get; set; }

    [StringLength(255)]
    public string? Organization { get; set; }

    public bool? IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastLoginAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    [InverseProperty("AssignedTo")]
    public virtual ICollection<Issue> IssueAssignedTos { get; set; } = new List<Issue>();

    [InverseProperty("Reporter")]
    public virtual ICollection<Issue> IssueReporters { get; set; } = new List<Issue>();

    [InverseProperty("VerifiedBy")]
    public virtual ICollection<Issue> IssueVerifiedBies { get; set; } = new List<Issue>();

    [InverseProperty("UpdatedBy")]
    public virtual ICollection<Issueupdate> Issueupdates { get; set; } = new List<Issueupdate>();

    [InverseProperty("User")]
    public virtual ICollection<Issuevote> Issuevotes { get; set; } = new List<Issuevote>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Userrole Role { get; set; } = null!;
}
