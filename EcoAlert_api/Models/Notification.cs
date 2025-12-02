using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("notifications")]
[Index("IssueId", Name = "IssueId")]
[Index("UserId", Name = "idx_user_notifications")]
public partial class Notification
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [Column(TypeName = "int(11)")]
    public int UserId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Message { get; set; } = null!;

    [StringLength(50)]
    public string? NotificationType { get; set; }

    [Column(TypeName = "int(11)")]
    public int? IssueId { get; set; }

    public bool? IsRead { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("IssueId")]
    [InverseProperty("Notifications")]
    public virtual Issue? Issue { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}
