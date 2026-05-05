using System.ComponentModel.DataAnnotations;

namespace GestionFetes.Models
{
    public class Invite
    {
        [Key]
        public int IdInvite { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères")]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [DataType(DataType.Date)]
        [Display(Name = "Date de naissance")]
        public DateTime DateNaissance { get; set; }

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        [StringLength(200, ErrorMessage = "L'adresse ne peut pas dépasser 200 caractères")]
        [Display(Name = "Adresse")]
        public string AdresseInvite { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Format email invalide")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Format téléphone invalide")]
        [Display(Name = "Téléphone")]
        public string? Telephone { get; set; }

        // Navigation
        public ICollection<Invitation> Invitations { get; set; } = new List<Invitation>();

        // Computed
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string NomComplet => $"{Prenom} {Nom}";

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public int Age => DateTime.Today.Year - DateNaissance.Year -
            (DateNaissance.Date > DateTime.Today.AddYears(-(DateTime.Today.Year - DateNaissance.Year)) ? 1 : 0);
    }
}
