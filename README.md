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

- **.NET SDK 10.0** ✅ (installé)
- **Flutter SDK** ✅ (installé dans /opt/flutter)
- **Git** (pour GitHub)

## Backend (.NET Web API)

**Note** : Pour utiliser `dotnet` dans les terminaux, ajoutez `export PATH="$HOME/.dotnet:$PATH"` à votre `~/.bashrc` ou exécutez cette commande avant d'utiliser dotnet.

### Lancer le backend

```bash
export PATH="$HOME/.dotnet:$PATH"
cd backend
dotnet run
```

L'API sera disponible sur : `https://localhost:7xxx` (le port exact s'affiche au démarrage)

### Tester l'API

Ouvrez dans le navigateur : `https://localhost:7xxx/swagger`

## Frontend (Flutter)

**Note** : Pour utiliser `flutter`, ajoutez `export PATH="/opt/flutter/bin:$PATH"` à votre `~/.bashrc` ou exécutez cette commande avant d'utiliser flutter.

### Lancer le frontend

```bash
export PATH="/opt/flutter/bin:$PATH"
cd frontend/agecanonix_app
flutter run -d web-server --web-port=8080
```

Voir les instructions complètes dans `frontend/README.md`.

## CI/CD avec GitHub Actions

Des workflows CI sont configurés dans `.github/workflows/` pour :
- Compiler et tester le backend .NET à chaque push
- (Flutter sera ajouté une fois installé)

## Prochaines étapes

1. ✅ Backend .NET créé et fonctionnel avec .NET 10
2. ✅ Flutter SDK installé et projet compilé
3. ⏳ Connecter le frontend au backend via API REST
4. ⏳ Ajouter des fonctionnalités métier

## License

Aucune licence pour le moment.
