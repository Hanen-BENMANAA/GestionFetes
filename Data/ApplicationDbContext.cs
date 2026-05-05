using GestionFetes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionFetes.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Fete> Fetes { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Salle> Salles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== FLUENT API =====

            // --- Salle ---
            modelBuilder.Entity<Salle>(entity =>
            {
                entity.HasKey(s => s.IdSalle);
                entity.Property(s => s.NomSalle)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(s => s.AddresseSalle)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(s => s.Capacite)
                      .IsRequired();
                entity.Property(s => s.PrixParHeure)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");
                entity.HasIndex(s => s.NomSalle).IsUnique();
            });

            // --- Fete ---
            modelBuilder.Entity<Fete>(entity =>
            {
                entity.HasKey(f => f.IdFete);
                entity.Property(f => f.Description)
                      .IsRequired()
                      .HasMaxLength(500);
                entity.Property(f => f.Type)
                      .IsRequired()
                      .HasConversion<string>();
                entity.Property(f => f.NbInvitesMax)
                      .IsRequired();
                entity.Property(f => f.Duree)
                      .IsRequired();
                entity.Property(f => f.DateFete)
                      .IsRequired();
                entity.HasOne(f => f.Salle)
                      .WithMany(s => s.Fetes)
                      .HasForeignKey(f => f.IdSalle)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // --- Invite ---
            modelBuilder.Entity<Invite>(entity =>
            {
                entity.HasKey(i => i.IdInvite);
                entity.Property(i => i.Nom)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(i => i.Prenom)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(i => i.DateNaissance)
                      .IsRequired();
                entity.Property(i => i.AdresseInvite)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(i => i.Email)
                      .HasMaxLength(150);
                entity.Property(i => i.Telephone)
                      .HasMaxLength(20);
            });

            // --- Invitation ---
            modelBuilder.Entity<Invitation>(entity =>
            {
                entity.HasKey(i => i.IdInvitation);
                entity.Property(i => i.DateInvitation)
                      .IsRequired();
                entity.Property(i => i.ConfirmeInvitation)
                      .IsRequired()
                      .HasDefaultValue(false);
                entity.HasOne(i => i.Fete)
                      .WithMany(f => f.Invitations)
                      .HasForeignKey(i => i.IdFete)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(i => i.Invite)
                      .WithMany(inv => inv.Invitations)
                      .HasForeignKey(i => i.IdInvite)
                      .OnDelete(DeleteBehavior.Restrict);
                // Unique: un invité ne peut être invité qu'une fois par fête
                entity.HasIndex(i => new { i.IdFete, i.IdInvite }).IsUnique();
            });

            // === SEED DATA ===
            modelBuilder.Entity<Salle>().HasData(
                new Salle { IdSalle = 1, NomSalle = "Salle Crystal", AddresseSalle = "12 Rue de la Paix, Tunis", Capacite = 200, PrixParHeure = 150.0 },
                new Salle { IdSalle = 2, NomSalle = "Palais des Roses", AddresseSalle = "45 Avenue Habib Bourguiba, Sfax", Capacite = 500, PrixParHeure = 300.0 },
                new Salle { IdSalle = 3, NomSalle = "Villa Jasmin", AddresseSalle = "8 Rue des Oliviers, Sousse", Capacite = 100, PrixParHeure = 100.0 }
            );

            modelBuilder.Entity<Fete>().HasData(
                new Fete { IdFete = 1, Description = "Anniversaire de Mohamed - 30 ans", Type = TypeFete.Anniversaire, NbInvitesMax = 80, Duree = 5, DateFete = DateTime.Now.AddDays(15), IdSalle = 1 },
                new Fete { IdFete = 2, Description = "Mariage de Sonia et Karim", Type = TypeFete.Mariage, NbInvitesMax = 300, Duree = 8, DateFete = DateTime.Now.AddDays(30), IdSalle = 2 },
                new Fete { IdFete = 3, Description = "Fête de fin d'année d'entreprise", Type = TypeFete.Autre, NbInvitesMax = 60, Duree = 4, DateFete = DateTime.Now.AddDays(7), IdSalle = 3 }
            );

            modelBuilder.Entity<Invite>().HasData(
                new Invite { IdInvite = 1, Nom = "Ben Ali", Prenom = "Ahmed", DateNaissance = new DateTime(1990, 5, 10), AdresseInvite = "Tunis Centre", Email = "ahmed@email.com", Telephone = "22345678" },
                new Invite { IdInvite = 2, Nom = "Trabelsi", Prenom = "Fatma", DateNaissance = new DateTime(1985, 3, 20), AdresseInvite = "La Marsa, Tunis", Email = "fatma@email.com", Telephone = "55123456" },
                new Invite { IdInvite = 3, Nom = "Khelil", Prenom = "Youssef", DateNaissance = new DateTime(1995, 8, 15), AdresseInvite = "Ariana, Tunis", Email = "youssef@email.com", Telephone = "98765432" }
            );
        }
    }
}
