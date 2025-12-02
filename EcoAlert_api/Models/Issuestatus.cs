using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("issuestatuses")]
[Index("Name", Name = "Name", IsUnique = true)]
public partial class Issuestatus
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();

    [InverseProperty("NewStatus")]
    public virtual ICollection<Issueupdate> IssueupdateNewStatuses { get; set; } = new List<Issueupdate>();

    [InverseProperty("OldStatus")]
    public virtual ICollection<Issueupdate> IssueupdateOldStatuses { get; set; } = new List<Issueupdate>();
}
