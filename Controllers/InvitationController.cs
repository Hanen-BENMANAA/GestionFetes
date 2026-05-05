using GestionFetes.Data;
using GestionFetes.Models;
using GestionFetes.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionFetes.Controllers
{
    [Authorize]
    public class InvitationController : Controller
    {
        private readonly IRepository<Invitation> _invitationRepo;
        private readonly IFeteRepository _feteRepo;
        private readonly IRepository<Invite> _inviteRepo;
        private readonly ApplicationDbContext _context;

        public InvitationController(IRepository<Invitation> invitationRepo,
                                     IFeteRepository feteRepo,
                                     IRepository<Invite> inviteRepo,
                                     ApplicationDbContext context)
        {
            _invitationRepo = invitationRepo;
            _feteRepo = feteRepo;
            _inviteRepo = inviteRepo;
            _context = context;
        }

        // Liste des invitations (pour l'user connecté - par email)
        public async Task<IActionResult> MesInvitations()
        {
            var email = User.Identity?.Name;
            var invitations = await _context.Invitations
                .Include(i => i.Fete).ThenInclude(f => f!.Salle)
                .Include(i => i.Invite)
                .Where(i => i.Invite!.Email == email)
                .OrderByDescending(i => i.DateInvitation)
                .ToListAsync();
            return View(invitations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmer(int id)
        {
            var invitation = await _invitationRepo.GetByIdAsync(id);
            if (invitation == null) return NotFound();

            invitation.ConfirmeInvitation = true;
            invitation.DateConfirmation = DateTime.Now;
            _invitationRepo.Update(invitation);
            await _invitationRepo.SaveAsync();

            TempData["Success"] = "Invitation confirmée avec succès !";
            return RedirectToAction("MesInvitations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Annuler(int id)
        {
            var invitation = await _invitationRepo.GetByIdAsync(id);
            if (invitation == null) return NotFound();

            invitation.ConfirmeInvitation = false;
            invitation.DateConfirmation = null;
            _invitationRepo.Update(invitation);
            await _invitationRepo.SaveAsync();

            TempData["Info"] = "Invitation annulée.";
            return RedirectToAction("MesInvitations");
        }
    }
}
