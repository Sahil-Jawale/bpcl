using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GRCAuthServicesMVC.Models;

[Table("ApplicationRole", Schema = "GRCAuth")]
public partial class ApplicationRole
{
    [Key]
    [Column("RoleId")]
    public int RoleId { get; set; }

    [StringLength(6)]
    [Unicode(false)]
    public string ApplicationId { get; set; } = null!;

    [Column("ApplicationRole")]
    [StringLength(30)]
    [Unicode(false)]
    public string? ApplicationRole1 { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string BusinessRole { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? AppAuthObject { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string ActiveStatus { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime UpdatedOn { get; set; }

    public int? UpdatedBy { get; set; }
}


