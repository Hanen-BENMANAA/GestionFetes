using GestionFetes.Data;
using GestionFetes.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionFetes.Repositories
{
    public class FeteRepository : Repository<Fete>, IFeteRepository
    {
        public FeteRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Fete>> GetFetesWithDetailsAsync()
        {
            return await _context.Fetes
                .Include(f => f.Salle)
                .Include(f => f.Invitations)
                    .ThenInclude(i => i.Invite)
                .OrderBy(f => f.DateFete)
                .ToListAsync();
        }

        public async Task<Fete?> GetFeteWithDetailsAsync(int id)
        {
            return await _context.Fetes
                .Include(f => f.Salle)
                .Include(f => f.Invitations)
                    .ThenInclude(i => i.Invite)
                .FirstOrDefaultAsync(f => f.IdFete == id);
        }

        public async Task<IEnumerable<Fete>> SearchFetesAsync(string? libelle, DateTime? dateDebut, DateTime? dateFin, TypeFete? type)
        {
            var query = _context.Fetes
                .Include(f => f.Salle)
                .Include(f => f.Invitations)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(libelle))
                query = query.Where(f => f.Description.Contains(libelle));

            if (dateDebut.HasValue)
                query = query.Where(f => f.DateFete >= dateDebut.Value);

            if (dateFin.HasValue)
                query = query.Where(f => f.DateFete <= dateFin.Value);

            if (type.HasValue)
                query = query.Where(f => f.Type == type.Value);

            return await query.OrderBy(f => f.DateFete).ToListAsync();
        }
    }
}
