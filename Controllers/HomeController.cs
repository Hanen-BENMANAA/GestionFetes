using GestionFetes.Models;
using GestionFetes.Repositories;
using GestionFetes.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionFetes.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IFeteRepository _feteRepo;

        public HomeController(IFeteRepository feteRepo)
        {
            _feteRepo = feteRepo;
        }

        public async Task<IActionResult> Index()
        {
            var fetes = await _feteRepo.GetFetesWithDetailsAsync();
            return View(fetes);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? libelle, DateTime? dateDebut, DateTime? dateFin, TypeFete? type)
        {
            var resultats = await _feteRepo.SearchFetesAsync(libelle, dateDebut, dateFin, type);
            var vm = new FeteSearchViewModel
            {
                Libelle = libelle,
                DateDebut = dateDebut,
                DateFin = dateFin,
                Type = type,
                Resultats = resultats
            };
            ViewBag.Types = new SelectList(Enum.GetValues<TypeFete>());
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var fete = await _feteRepo.GetFeteWithDetailsAsync(id);
            if (fete == null) return NotFound();
            return View(fete);
        }

        [AllowAnonymous]
        public IActionResult Error() => View();
    }
}
