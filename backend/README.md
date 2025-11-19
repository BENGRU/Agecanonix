# Agecanonix Backend API

API de gestion administrative et de facturation pour les EHPAD en France.

## 🏗️ Architecture

Le projet suit les principes de **Clean Architecture** avec une séparation claire des responsabilités :

```
backend/
├── Agecanonix.Domain/           # Entités métier et règles de domaine
│   ├── Entities/                # Établissement, Résident, Facture, Personnel
│   └── Common/                  # Classes de base (BaseEntity)
│
├── Agecanonix.Application/      # Logique applicative et use cases
│   ├── DTOs/                    # Data Transfer Objects
│   └── ...                      # (MediatR handlers, validators à venir)
│
├── Agecanonix.Infrastructure/   # Implémentations techniques
│   └── Data/                    # DbContext, repositories, migrations
│
└── Agecanonix.Api/             # API REST et configuration
    ├── Program.cs               # Configuration de l'application
    └── appsettings.json         # Configuration (DB, JWT, Serilog)
```

## 🛠️ Technologies

- **.NET 10** - Framework backend moderne
- **Entity Framework Core 10** - ORM pour l'accès aux données
- **PostgreSQL** - Base de données relationnelle
- **JWT Authentication** - Sécurité API
- **Serilog** - Logging structuré
- **MediatR** - Pattern CQRS et mediator
- **FluentValidation** - Validation des données
- **AutoMapper** - Mapping d'objets

## 📋 Prérequis

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- Un IDE : Visual Studio 2022, VS Code, ou Rider

## 🚀 Installation

### 1. Cloner le repository
```bash
git clone https://github.com/BENGRU/Agecanonix.git
cd Agecanonix/backend
```

### 2. Installer PostgreSQL
```bash
# Ubuntu/Debian
sudo apt-get install postgresql postgresql-contrib

# macOS
brew install postgresql

# Windows
# Télécharger depuis https://www.postgresql.org/download/windows/
```

### 3. Créer la base de données
```bash
sudo -u postgres psql
CREATE DATABASE agecanonix;
CREATE USER agecanonix_user WITH PASSWORD 'votre_mot_de_passe';
GRANT ALL PRIVILEGES ON DATABASE agecanonix TO agecanonix_user;
\q
```

### 4. Configurer la connection string
Éditez `appsettings.json` :
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=agecanonix;Username=agecanonix_user;Password=votre_mot_de_passe"
  }
}
```

### 5. Créer les migrations
```bash
# Installer l'outil EF Core si nécessaire
dotnet tool install --global dotnet-ef

# Créer la migration initiale
dotnet ef migrations add InitialCreate --project Agecanonix.Infrastructure --startup-project Agecanonix.Api

# Appliquer la migration
dotnet ef database update --project Agecanonix.Infrastructure --startup-project Agecanonix.Api
```

### 6. Lancer l'application
```bash
dotnet run --project Agecanonix.Api
```

L'API sera accessible sur `http://localhost:5000`

## 📚 Endpoints API

### Racine
- `GET /` - Informations sur l'API
- `GET /health` - Health check
- `GET /openapi/v1.json` - Spécification OpenAPI

### À venir
- `/api/etablissements` - Gestion des établissements
- `/api/residents` - Gestion des résidents
- `/api/factures` - Gestion de la facturation
- `/api/personnel` - Gestion du personnel

## 🔒 Sécurité

### JWT Authentication
L'API utilise JWT pour l'authentification. Configuration dans `appsettings.json` :

```json
{
  "JwtSettings": {
    "SecretKey": "VotreCleSuperSecreteQuiDoitEtreLongue2024!",
    "Issuer": "AgecanonixAPI",
    "Audience": "AgecanonixClient",
    "ExpirationMinutes": 60
  }
}
```

**⚠️ Important** : Changez la `SecretKey` en production !

## 📊 Modèle de données

### Établissement
- Informations administratives (SIRET, adresse, capacité)
- Relations avec résidents et personnel

### Résident
- Informations personnelles et médicales
- Historique des séjours et factures
- Contact d'urgence

### Facture
- Numéro, dates, montants (HT/TTC)
- Statut de paiement
- Lignes de prestation détaillées

### Personnel
- Informations du personnel
- Fonction et affectation à l'établissement

## 🧪 Tests

```bash
# Lancer les tests unitaires (à venir)
dotnet test

# Avec couverture de code (à venir)
dotnet test /p:CollectCoverage=true
```

## 📝 Logs

Les logs sont générés avec Serilog :
- Console : logs en temps réel
- Fichiers : `logs/agecanonix-YYYYMMDD.log`

## 🌐 Déploiement

### Azure
```bash
# Créer une base PostgreSQL sur Azure
az postgres flexible-server create --resource-group agecanonix-rg --name agecanonix-db

# Déployer l'API sur Azure App Service
az webapp up --name agecanonix-api --resource-group agecanonix-rg
```

### Docker (à venir)
```bash
docker build -t agecanonix-api .
docker run -p 5000:5000 agecanonix-api
```

## 🛣️ Roadmap

- [x] Architecture Clean Architecture
- [x] Modèle de données de base
- [x] Configuration EF Core + PostgreSQL
- [x] Authentification JWT
- [x] Logging Serilog
- [ ] Endpoints CRUD complets
- [ ] Gestion des utilisateurs
- [ ] Multi-tenancy (isolation par établissement)
- [ ] Génération de factures PDF
- [ ] Exports comptables
- [ ] Tests unitaires et d'intégration
- [ ] Documentation Swagger/OpenAPI complète
- [ ] CI/CD avec GitHub Actions

## 📄 Licence

À définir

## 🤝 Contribution

Contributions bienvenues ! Créez une issue ou une pull request.

## 📧 Contact

Pour toute question : [votre-email@example.com]
