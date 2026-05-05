# GestionFêtes - Mini Projet DOTNET

## Prérequis
- .NET 8 SDK : https://dotnet.microsoft.com/download
- Visual Studio 2022 (ou VS Code)

## Lancer le projet

### Étape 1 : Ouvrir le projet
```
Ouvrez GestionFetes.sln dans Visual Studio
```

### Étape 2 : Restaurer les packages NuGet
```
dotnet restore
```

### Étape 3 : Appliquer les migrations (créer la base de données)
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Ou dans la Console du gestionnaire de packages (Visual Studio) :
```
Add-Migration InitialCreate
Update-Database
```

### Étape 4 : Lancer l'application
```
dotnet run
```
Ou appuyez sur F5 dans Visual Studio.

## Compte administrateur par défaut
- **Email** : admin@fetes.tn
- **Mot de passe** : Admin@123

## Structure du projet
```
GestionFetes/
├── Controllers/
│   ├── AccountController.cs   (Login, Register, Logout)
│   ├── HomeController.cs      (Liste fêtes, Recherche, Détails)
│   ├── InvitationController.cs (Mes invitations, Confirmer)
│   └── AdminController.cs     (CRUD complet + Dashboard)
├── Models/
│   ├── Fete.cs
│   ├── Invite.cs
│   ├── Invitation.cs
│   ├── Salle.cs
│   ├── TypeFete.cs (enum)
│   └── ApplicationUser.cs
├── Data/
│   ├── ApplicationDbContext.cs (EF Core + Fluent API + Seed)
│   └── DbInitializer.cs (Rôles + Admin par défaut)
├── Repositories/
│   ├── IRepository.cs (Repository Pattern générique)
│   ├── Repository.cs
│   ├── IFeteRepository.cs
│   └── FeteRepository.cs
├── Services/
│   └── StatistiquesService.cs
├── ViewModels/
│   └── ViewModels.cs
└── Views/
    ├── Account/ (Login, Register, AccessDenied)
    ├── Home/ (Index, Search, Details)
    ├── Invitation/ (MesInvitations)
    └── Admin/ (Dashboard, Fetes CRUD, Salles CRUD, Invites CRUD, Invitations, Users)
```

## Fonctionnalités
- ✅ Authentification (Login / Register / Logout)
- ✅ Rôles : Admin et User
- ✅ CRUD complet : Fêtes, Salles, Invités, Invitations
- ✅ Recherche de fêtes (par libellé, date, type)
- ✅ Confirmation d'invitations
- ✅ Dashboard avec statistiques et graphiques (Chart.js)
- ✅ Annotations + Fluent API
- ✅ Repository Pattern (patron de conception)
- ✅ Seed data (données initiales)
- ✅ SQLite (facile, pas besoin de SQL Server)

## Patrons de conception utilisés
- **Repository Pattern** : IRepository<T> + FeteRepository
- **Dependency Injection** : Injection via Program.cs
- **MVC Pattern** : Architecture complète ASP.NET Core MVC
