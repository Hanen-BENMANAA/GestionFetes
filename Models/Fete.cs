using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionFetes.Models
{
    public class Fete
    {
        [Key]
        public int IdFete { get; set; }

        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "La description doit contenir entre 5 et 500 caractères")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le type de fête est obligatoire")]
        [Display(Name = "Type de fête")]
        public TypeFete Type { get; set; }

        [Required(ErrorMessage = "Le nombre max d'invités est obligatoire")]
        [Range(1, 10000, ErrorMessage = "Le nombre doit être entre 1 et 10000")]
        [Display(Name = "Nombre max d'invités")]
        public int NbInvitesMax { get; set; }

        [Required(ErrorMessage = "La durée est obligatoire")]
        [Range(1, 72, ErrorMessage = "La durée doit être entre 1 et 72 heures")]
        [Display(Name = "Durée (heures)")]
        public int Duree { get; set; }

        [Required(ErrorMessage = "La date est obligatoire")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date de la fête")]
        public DateTime DateFete { get; set; }

        // FK
        [Required(ErrorMessage = "La salle est obligatoire")]
        [Display(Name = "Salle")]
        public int IdSalle { get; set; }

        [ForeignKey("IdSalle")]
        public Salle? Salle { get; set; }

        // Navigation
        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        // Computed
        [NotMapped]
        public int NbInvitesConfirmes => Invitations.Count(i => i.ConfirmeInvitation);

        [NotMapped]
        public double CoutTotal => Duree * (Salle?.PrixParHeure ?? 0);
    }
}
