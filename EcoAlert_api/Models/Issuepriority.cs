using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("issuepriorities")]
[Index("Name", Name = "Name", IsUnique = true)]
public partial class Issuepriority
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "int(11)")]
    public int? ScoreRangeMin { get; set; }

    [Column(TypeName = "int(11)")]
    public int? ScoreRangeMax { get; set; }

    [InverseProperty("Priority")]
    public virtual ICollection<Issue> Issues { get; set; } = new List<Issue>();
}
