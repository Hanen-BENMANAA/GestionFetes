using GestionFetes.Data;
using GestionFetes.Models;
using GestionFetes.Repositories;
using GestionFetes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GestionFetes.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IFeteRepository _feteRepo;
        private readonly IRepository<Salle> _salleRepo;
        private readonly IRepository<Invite> _inviteRepo;
        private readonly IRepository<Invitation> _invitationRepo;
        private readonly IStatistiquesService _statsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(IFeteRepository feteRepo,
                                IRepository<Salle> salleRepo,
                                IRepository<Invite> inviteRepo,
                                IRepository<Invitation> invitationRepo,
                                IStatistiquesService statsService,
                                UserManager<ApplicationUser> userManager,
                                ApplicationDbContext context)
        {
            _feteRepo = feteRepo;
            _salleRepo = salleRepo;
            _inviteRepo = inviteRepo;
            _invitationRepo = invitationRepo;
            _statsService = statsService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
        {
            var stats = await _statsService.GetStatistiquesAsync();
            return View(stats);
        }

        // ========== FETES CRUD ==========
        public async Task<IActionResult> Fetes()
        {
            var fetes = await _feteRepo.GetFetesWithDetailsAsync();
            return View(fetes);
        }

        public async Task<IActionResult> CreateFete()
        {
            ViewBag.Salles = new SelectList(await _salleRepo.GetAllAsync(), "IdSalle", "NomSalle");
            ViewBag.Types = new SelectList(Enum.GetValues<TypeFete>());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFete(Fete fete)
        {
            if (ModelState.IsValid)
            {
                await _feteRepo.AddAsync(fete);
                await _feteRepo.SaveAsync();
                TempData["Success"] = "Fête créée avec succès !";
                return RedirectToAction("Fetes");
            }
            ViewBag.Salles = new SelectList(await _salleRepo.GetAllAsync(), "IdSalle", "NomSalle");
            ViewBag.Types = new SelectList(Enum.GetValues<TypeFete>());
            return View(fete);
        }

        public async Task<IActionResult> EditFete(int id)
        {
            var fete = await _feteRepo.GetByIdAsync(id);
            if (fete == null) return NotFound();
            ViewBag.Salles = new SelectList(await _salleRepo.GetAllAsync(), "IdSalle", "NomSalle", fete.IdSalle);
            ViewBag.Types = new SelectList(Enum.GetValues<TypeFete>(), fete.Type);
            return View(fete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFete(Fete fete)
        {
            if (ModelState.IsValid)
            {
                _feteRepo.Update(fete);
                await _feteRepo.SaveAsync();
                TempData["Success"] = "Fête modifiée avec succès !";
                return RedirectToAction("Fetes");
            }
            ViewBag.Salles = new SelectList(await _salleRepo.GetAllAsync(), "IdSalle", "NomSalle");
            ViewBag.Types = new SelectList(Enum.GetValues<TypeFete>());
            return View(fete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFete(int id)
        {
            var fete = await _feteRepo.GetByIdAsync(id);
            if (fete == null) return NotFound();
            _feteRepo.Delete(fete);
            await _feteRepo.SaveAsync();
            TempData["Success"] = "Fête supprimée.";
            return RedirectToAction("Fetes");
        }

        // ========== SALLES CRUD ==========
        public async Task<IActionResult> Salles()
        {
            var salles = await _salleRepo.GetAllAsync();
            return View(salles);
        }

        public IActionResult CreateSalle() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSalle(Salle salle)
        {
            if (ModelState.IsValid)
            {
                await _salleRepo.AddAsync(salle);
                await _salleRepo.SaveAsync();
                TempData["Success"] = "Salle créée !";
                return RedirectToAction("Salles");
            }
            return View(salle);
        }

        public async Task<IActionResult> EditSalle(int id)
        {
            var salle = await _salleRepo.GetByIdAsync(id);
            if (salle == null) return NotFound();
            return View(salle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSalle(Salle salle)
        {
            if (ModelState.IsValid)
            {
                _salleRepo.Update(salle);
                await _salleRepo.SaveAsync();
                TempData["Success"] = "Salle modifiée !";
                return RedirectToAction("Salles");
            }
            return View(salle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSalle(int id)
        {
            var salle = await _salleRepo.GetByIdAsync(id);
            if (salle == null) return NotFound();
            _salleRepo.Delete(salle);
            await _salleRepo.SaveAsync();
            TempData["Success"] = "Salle supprimée.";
            return RedirectToAction("Salles");
        }

        // ========== INVITES CRUD ==========
        public async Task<IActionResult> Invites()
        {
            var invites = await _inviteRepo.GetAllAsync();
            return View(invites);
        }

        public IActionResult CreateInvite() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInvite(Invite invite)
        {
            if (ModelState.IsValid)
            {
                await _inviteRepo.AddAsync(invite);
                await _inviteRepo.SaveAsync();
                TempData["Success"] = "Invité ajouté !";
                return RedirectToAction("Invites");
            }
            return View(invite);
        }

        public async Task<IActionResult> EditInvite(int id)
        {
            var invite = await _inviteRepo.GetByIdAsync(id);
            if (invite == null) return NotFound();
            return View(invite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInvite(Invite invite)
        {
            if (ModelState.IsValid)
            {
                _inviteRepo.Update(invite);
                await _inviteRepo.SaveAsync();
                TempData["Success"] = "Invité modifié !";
                return RedirectToAction("Invites");
            }
            return View(invite);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInvite(int id)
        {
            var invite = await _inviteRepo.GetByIdAsync(id);
            if (invite == null) return NotFound();
            _inviteRepo.Delete(invite);
            await _inviteRepo.SaveAsync();
            TempData["Success"] = "Invité supprimé.";
            return RedirectToAction("Invites");
        }

        // ========== INVITATIONS CRUD ==========
        public async Task<IActionResult> Invitations()
        {
            var invitations = await _context.Invitations
                .Include(i => i.Fete)
                .Include(i => i.Invite)
                .OrderByDescending(i => i.DateInvitation)
                .ToListAsync();
            return View(invitations);
        }

        public async Task<IActionResult> CreateInvitation()
        {
            ViewBag.Fetes = new SelectList(await _feteRepo.GetAllAsync(), "IdFete", "Description");
            ViewBag.Invites = new SelectList(await _inviteRepo.GetAllAsync(), "IdInvite", "NomComplet");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInvitation(Invitation invitation)
        {
            // Check if already invited
            var exists = await _context.Invitations
                .AnyAsync(i => i.IdFete == invitation.IdFete && i.IdInvite == invitation.IdInvite);
            if (exists)
            {
                ModelState.AddModelError("", "Cet invité est déjà invité à cette fête.");
            }

            if (ModelState.IsValid)
            {
                invitation.DateInvitation = DateTime.Now;
                await _invitationRepo.AddAsync(invitation);
                await _invitationRepo.SaveAsync();
                TempData["Success"] = "Invitation créée !";
                return RedirectToAction("Invitations");
            }
            ViewBag.Fetes = new SelectList(await _feteRepo.GetAllAsync(), "IdFete", "Description");
            ViewBag.Invites = new SelectList(await _inviteRepo.GetAllAsync(), "IdInvite", "NomComplet");
            return View(invitation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteInvitation(int id)
        {
            var inv = await _invitationRepo.GetByIdAsync(id);
            if (inv == null) return NotFound();
            _invitationRepo.Delete(inv);
            await _invitationRepo.SaveAsync();
            TempData["Success"] = "Invitation supprimée.";
            return RedirectToAction("Invitations");
        }

        // ========== USERS ==========
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<string, IList<string>>();
            foreach (var user in users)
                userRoles[user.Id] = await _userManager.GetRolesAsync(user);
            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            await _userManager.DeleteAsync(user);
            TempData["Success"] = "Utilisateur supprimé.";
            return RedirectToAction("Users");
        }
    }
}
