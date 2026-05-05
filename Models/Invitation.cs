using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionFetes.Models
{
    public class Invitation
    {
        [Key]
        public int IdInvitation { get; set; }

        [Required(ErrorMessage = "La date d'invitation est obligatoire")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date d'invitation")]
        public DateTime DateInvitation { get; set; } = DateTime.Now;

        [Display(Name = "Invitation confirmée")]
        public bool ConfirmeInvitation { get; set; } = false;

        [DataType(DataType.DateTime)]
        [Display(Name = "Date de confirmation")]
        public DateTime? DateConfirmation { get; set; }

        // FK
        [Required]
        public int IdFete { get; set; }

        [ForeignKey("IdFete")]
        public Fete? Fete { get; set; }

        [Required]
        public int IdInvite { get; set; }

        [ForeignKey("IdInvite")]
        public Invite? Invite { get; set; }
    }
}
