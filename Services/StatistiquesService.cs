using GestionFetes.Data;
using GestionFetes.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionFetes.Services
{
    public interface IStatistiquesService
    {
        Task<StatistiquesViewModel> GetStatistiquesAsync();
    }

    public class StatistiquesViewModel
    {
        public int TotalFetes { get; set; }
        public int TotalInvites { get; set; }
        public int TotalInvitations { get; set; }
        public int TotalInvitationsConfirmees { get; set; }
        public int TotalSalles { get; set; }
        public double TauxConfirmation { get; set; }
        public Dictionary<string, int> FetesParType { get; set; } = new();
        public Dictionary<string, int> FetesParMois { get; set; } = new();
        public List<(string Salle, int NbFetes)> TopSalles { get; set; } = new();
        public Fete? ProchainesFete { get; set; }
        public double RevenuTotal { get; set; }
    }

    public class StatistiquesService : IStatistiquesService
    {
        private readonly ApplicationDbContext _context;

        public StatistiquesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StatistiquesViewModel> GetStatistiquesAsync()
        {
            var fetes = await _context.Fetes
                .Include(f => f.Salle)
                .Include(f => f.Invitations)
                .ToListAsync();

            var invitations = await _context.Invitations.ToListAsync();
            var invites = await _context.Invites.ToListAsync();
            var salles = await _context.Salles.ToListAsync();

            var vm = new StatistiquesViewModel
            {
                TotalFetes = fetes.Count,
                TotalInvites = invites.Count,
                TotalInvitations = invitations.Count,
                TotalInvitationsConfirmees = invitations.Count(i => i.ConfirmeInvitation),
                TotalSalles = salles.Count,
                TauxConfirmation = invitations.Count > 0
                    ? Math.Round((double)invitations.Count(i => i.ConfirmeInvitation) / invitations.Count * 100, 1)
                    : 0,
                FetesParType = fetes.GroupBy(f => f.Type.ToString())
                    .ToDictionary(g => g.Key, g => g.Count()),
                FetesParMois = fetes
                    .GroupBy(f => f.DateFete.ToString("MMM yyyy"))
                    .ToDictionary(g => g.Key, g => g.Count()),
                TopSalles = fetes.GroupBy(f => f.Salle?.NomSalle ?? "Inconnue")
                    .Select(g => (g.Key, g.Count()))
                    .OrderByDescending(x => x.Item2)
                    .Take(5)
                    .ToList(),
                ProchainesFete = fetes.Where(f => f.DateFete > DateTime.Now)
                    .OrderBy(f => f.DateFete)
                    .FirstOrDefault(),
                RevenuTotal = fetes.Sum(f => f.Duree * (f.Salle?.PrixParHeure ?? 0))
            };

            return vm;
        }
    }
}
