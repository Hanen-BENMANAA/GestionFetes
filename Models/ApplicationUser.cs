using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GestionFetes.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Nom complet")]
        public string NomComplet { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Date d'inscription")]
        public DateTime DateInscription { get; set; } = DateTime.Now;
    }
}
