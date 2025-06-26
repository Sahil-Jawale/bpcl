using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
namespace GRCAuthServicesMVC.Models.DTO
{
    public class ApplicationInventoryRepository
    {


        [Column("AppInvID")]
        public int AppInvId { get; set; }

        [Column("ApplicationID")]
        [StringLength(6)]
        [Unicode(false)]
        public string ApplicationId { get; set; } = null!;

        [StringLength(500)]
        [Unicode(false)]
        public string? ApplicationDesc { get; set; }

        [StringLength(100)]
        [Unicode(false)]
        public string ProcessOwner { get; set; } = null!;

        [StringLength(1)]
        [Unicode(false)]
        public string ActiveStatus { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime UpdatedOn { get; set; }

        public int UpdatedBy { get; set; }





        public List<ApplicationInventory> ApplicationInventoryList { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }


    }
}
