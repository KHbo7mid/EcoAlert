using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("votetypes")]
[Index("Name", Name = "Name", IsUnique = true)]
public partial class Votetype
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [StringLength(20)]
    public string Name { get; set; } = null!;

    [InverseProperty("VoteType")]
    public virtual ICollection<Issuevote> Issuevotes { get; set; } = new List<Issuevote>();
}
