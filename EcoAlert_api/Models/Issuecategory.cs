using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EcoAlert.Models;

[Table("issuecategories")]
[Index("Name", Name = "Name", IsUnique = true)]
public partial class Issuecategory
{
    [Key]
    [Column(TypeName = "int(11)")]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Precision(3, 2)]
    public decimal? PriorityWeight { get; set; }

    [InverseProperty("SuggestedCategory")]
    public virtual ICollection<Aianalysis> Aianalyses { get; set; } = new List<Aianalysis>();

    [InverseProperty("AisuggestedCategory")]
    public virtual ICollection<Issue> IssueAisuggestedCategories { get; set; } = new List<Issue>();

    [InverseProperty("Category")]
    public virtual ICollection<Issue> IssueCategories { get; set; } = new List<Issue>();
}
