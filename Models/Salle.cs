using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionFetes.Models
{
    public class Salle
    {
        [Key]
        public int IdSalle { get; set; }

        [Required(ErrorMessage = "Le nom de la salle est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom de la salle")]
        public string NomSalle { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        [StringLength(200, ErrorMessage = "L'adresse ne peut pas dépasser 200 caractères")]
        [Display(Name = "Adresse")]
        public string AddresseSalle { get; set; } = string.Empty;

        [Required(ErrorMessage = "La capacité est obligatoire")]
        [Range(1, 10000, ErrorMessage = "La capacité doit être entre 1 et 10000")]
        [Display(Name = "Capacité")]
        public int Capacite { get; set; }

        [Required(ErrorMessage = "Le prix par heure est obligatoire")]
        [Range(0.01, 100000, ErrorMessage = "Le prix doit être positif")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Prix par heure (DT)")]
        public double PrixParHeure { get; set; }

        // Navigation
        public ICollection<Fete> Fetes { get; set; } = new List<Fete>();
    }
}
