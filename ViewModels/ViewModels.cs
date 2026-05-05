using System.ComponentModel.DataAnnotations;
using GestionFetes.Models;

namespace GestionFetes.ViewModels
{
    // ===== AUTH =====
    public class LoginViewModel
    {
        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format email invalide")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Se souvenir de moi")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Le nom complet est obligatoire")]
        [StringLength(100)]
        [Display(Name = "Nom complet")]
        public string NomComplet { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Format email invalide")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
        [Display(Name = "Confirmer le mot de passe")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    // ===== FETE SEARCH =====
    public class FeteSearchViewModel
    {
        public string? Libelle { get; set; }
        public DateTime? DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public TypeFete? Type { get; set; }
        public IEnumerable<Fete> Resultats { get; set; } = new List<Fete>();
    }

    // ===== INVITATION CONFIRM =====
    public class ConfirmerInvitationViewModel
    {
        public int IdInvitation { get; set; }
        public string NomInvite { get; set; } = string.Empty;
        public string DescriptionFete { get; set; } = string.Empty;
        public DateTime DateFete { get; set; }
        public bool ConfirmeInvitation { get; set; }
    }
}
