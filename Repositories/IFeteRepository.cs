using GestionFetes.Models;

namespace GestionFetes.Repositories
{
    public interface IFeteRepository : IRepository<Fete>
    {
        Task<IEnumerable<Fete>> GetFetesWithDetailsAsync();
        Task<Fete?> GetFeteWithDetailsAsync(int id);
        Task<IEnumerable<Fete>> SearchFetesAsync(string? libelle, DateTime? dateDebut, DateTime? dateFin, TypeFete? type);
    }
}
