# Agecanonix

Projet full-stack avec backend .NET et frontend Flutter.

## Structure du projet

```
Agecanonix/
├── backend/          # API ASP.NET Core Web API (.NET 9)
├── frontend/         # Application Flutter
├── .github/          # Configuration GitHub Actions
└── README.md
```

## Prérequis

- **.NET SDK 9.0** ✅ (installé)
- **Flutter SDK** ⚠️ (à installer - voir frontend/README.md)
- **Git** (pour GitHub)

## Backend (.NET Web API)

### Lancer le backend

```powershell
cd backend
dotnet run
```

L'API sera disponible sur : `https://localhost:7xxx` (le port exact s'affiche au démarrage)

### Tester l'API

Ouvrez dans le navigateur : `https://localhost:7xxx/swagger`

## Frontend (Flutter)

Voir les instructions dans `frontend/README.md` pour installer Flutter et créer l'application.

## CI/CD avec GitHub Actions

Des workflows CI sont configurés dans `.github/workflows/` pour :
- Compiler et tester le backend .NET à chaque push
- (Flutter sera ajouté une fois installé)

## Prochaines étapes

1. ✅ Backend .NET créé et fonctionnel
2. ⏳ Installer Flutter SDK (voir frontend/README.md)
3. ⏳ Créer le projet Flutter dans `frontend/`
4. ⏳ Initialiser Git et créer le dépôt GitHub
5. ⏳ Configurer GitHub Actions

## License

Aucune licence pour le moment.
