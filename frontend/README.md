# Frontend Flutter - Agecanonix

## Installation de Flutter

Flutter SDK est installé dans `/opt/flutter`.

Pour utiliser Flutter, ajoutez-le au PATH :
```bash
export PATH="/opt/flutter/bin:$PATH"
```

## Lancer l'application (Web)

L'application Flutter est prête dans `frontend/agecanonix_app`.

### Installer les dépendances
```bash
export PATH="/opt/flutter/bin:$PATH"
cd /workspaces/Agecanonix/frontend/agecanonix_app
flutter pub get
```

### Exécuter en mode développement
```bash
flutter run -d web-server --web-port=8080
```

### Compiler pour la production
```bash
flutter build web --release
```

L'application compilée sera dans `build/web/`.

## Structure prévue

Le frontend Flutter communiquera avec le backend .NET via API REST.

## Dépendances utiles (à venir)

- http (requêtes API)
- flutter_riverpod ou provider (état)
- go_router (navigation)
