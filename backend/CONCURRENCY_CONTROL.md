# Versionning de Ligne - Contrôle de Concurrence Optimiste

## Vue d'ensemble

Un système de versionning de ligne (row versioning) a été implémenté pour prévenir les conflits d'accès concurrents lors de la mise à jour des entités. Cela utilise le contrôle de concurrence **optimiste**, où les changements conflictuels sont détectés et gérés au moment de la sauvegarde plutôt que d'être bloqués à l'avance.

## Architecture

### 1. **Colonne RowVersion dans les Entités**

Chaque entité héritant de `BaseEntity` possède une propriété `RowVersion` de type `byte[]` annotée avec `[Timestamp]`:

```csharp
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// Row version for optimistic concurrency control
    /// </summary>
    [System.ComponentModel.DataAnnotations.Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
```

**Caractéristiques de RowVersion :**
- Automatiquement mis à jour par Entity Framework Core à chaque modification
- Utilisé par Entity Framework pour détecter les conflits de concurrence
- Stocké comme `BLOB` en SQLite ou `ROWVERSION` en SQL Server

### 2. **Configuration Entity Framework Core**

Chaque configuration d'entité inclut la configuration explicite de `RowVersion`:

```csharp
builder.Property(e => e.RowVersion)
    .IsRowVersion();
```

**Entités configurées :**
- `Facility` (FacilityConfiguration)
- `Individual` (IndividualConfiguration)
- `IndividualRelationship` (IndividualRelationshipConfiguration)
- `ServiceType` (ServiceTypeConfiguration)
- `TargetPopulation` (TargetPopulationConfiguration)

### 3. **DTOs avec RowVersion**

Le `RowVersion` est inclus dans les DTOs pour être communiqué au client:

**DTOs de Réponse (read):**
- `IndividualDto`
- `FacilityDto`
- `IndividualRelationshipDto`
- `ServiceTypeDto`
- `TargetPopulationDto`

**DTOs de Mise à Jour (write):**
- `UpdateIndividualDto`
- `UpdateFacilityDto`
- `UpdateIndividualRelationshipDto`
- `UpdateServiceTypeDto`
- `UpdateTargetPopulationDto`

### 4. **Gestion des Exceptions de Concurrence**

#### Exception Personnalisée

```csharp
public class ConcurrencyException : ApplicationException
{
    // Exception levée quand une violation de concurrence optimiste est détectée
}
```

#### CommandHandlers Mis à Jour

Les handlers de mise à jour ont été modifiés pour:

1. **Définir le RowVersion depuis le DTO:**
   ```csharp
   entity.RowVersion = request.Dto.RowVersion;
   ```

2. **Capturer les exceptions de concurrence:**
   ```csharp
   try
   {
       request.Dto.Adapt(entity);
       entity.UpdatedAt = DateTime.UtcNow;
       entity.UpdatedBy = "system";
       await _repository.UpdateAsync(entity, cancellationToken);
   }
   catch (DbUpdateConcurrencyException ex)
   {
       throw new ConcurrencyException(
           $"Unable to update {Entity} with ID {id}. " +
           $"The record was modified by another user. Please refresh and try again.",
           ex
       );
   }
   ```

**Handlers mis à jour:**
- `UpdateIndividualCommandHandler`
- `UpdateFacilityCommandHandler`
- `UpdateIndividualRelationshipCommandHandler`
- `UpdateServiceTypeCommandHandler`
- `UpdateTargetPopulationCommandHandler`

## Flux de Travail - Exemple

### Étape 1: Lecture (GET)
Le client récupère l'entité avec son `RowVersion`:
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Alice",
  "rowVersion": "AQAAAAA=" // Base64 encoded byte array
}
```

### Étape 2: Modification Client
L'utilisateur modifie l'entité localement dans son application.

### Étape 3: Mise à Jour (PUT) avec RowVersion
Le client envoie la mise à jour **avec le RowVersion original**:
```json
{
  "name": "Alice Smith",
  "rowVersion": "AQAAAAA=" // Le RowVersion d'origine
}
```

### Étape 4: Vérification de Concurrence
Entity Framework Core compare le RowVersion envoyé avec la version actuelle en base de données:

**Cas A - Pas de Concurrence:** ✅
- Les versions correspondent
- La mise à jour est appliquée
- `RowVersion` est automatiquement mis à jour par la base de données
- Réponse HTTP 200 OK

**Cas B - Conflit de Concurrence:** ❌
- Les versions ne correspondent pas (quelqu'un d'autre a modifié l'entité)
- `DbUpdateConcurrencyException` est levée
- Convertie en `ConcurrencyException`
- Réponse HTTP 409 Conflict
- Le client doit rafraîchir et réessayer

## Migration Database

Une migration a été créée pour ajouter la colonne `RowVersion` à toutes les tables existantes:

```bash
dotnet ef migrations add AddRowVersionConcurrencyControl
```

**Nom du fichier de migration:**
- `20260214100017_AddRowVersionConcurrencyControl.cs`

La colonne est créée en tant que `BLOB` (SQLite) ou `ROWVERSION` (SQL Server) avec le paramètre `rowVersion: true`.

## Avantages du Versionning de Ligne

✅ **Prévention des Lost Updates**
- Détecte quand une autre personne a modifié l'entité
- Évite que les changements d'un utilisateur écrasent ceux d'un autre

✅ **Pas de Blocages**
- Les lecteurs ne bloquent pas les écrivains
- Les écrivains ne bloquent pas les lecteurs
- Concurrence optimiste

✅ **Messages d'Erreur Clairs**
- Le client reçoit une exception `ConcurrencyException`
- Message indiquant que le résultat a été modifié

✅ **Gestion Coté Client**
- L'application cliente peut afficher un message
- Proposer de rafraîchir et réessayer
- Ou fusionner les changements intelligemment

## Intégration Frontend

Pour implémenter cela dans Flutter:

```dart
// 1. Stocker le RowVersion reçu du serveur
String? currentRowVersion;

// 2. Lors de la modification, envoyer le RowVersion dans la requête
final updatePayload = {
  'name': nameController.text,
  'rowVersion': currentRowVersion,
};

// 3. Gérer l'erreur 409 Conflict
try {
  final response = await api.put('/api/individuals/$id', updatePayload);
  // Succès - mettre à jour local et RowVersion
  currentRowVersion = response.body['rowVersion'];
} catch (e) {
  if (e.statusCode == 409) {
    // Afficher un message: "La donnée a été modifiée par quelqu'un d'autre"
    // Proposer de rafraîchir
  }
}
```

## Considérations Futures

1. **Stratégie de Fusion**: Implémenter une fusion intelligente des changements en cas de conflit
2. **Historique des Versions**: Peut-être conserver un historique complet des modifications
3. **Audit**: Tracer qui a modifié quelle entité à quel moment
4. **Événements de Domaine**: Émettre des événements quand un conflit est détecté

## Ressources

- [Entity Framework Core - Optimistic Concurrency](https://learn.microsoft.com/en-us/ef/core/saving/concurrency)
- [Concurrency Patterns](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-optimistic-concurrency-based-on-multiple-versions-of-files)
