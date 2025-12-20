using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.DTOs.Individual;
using Agecanonix.Application.DTOs.IndividualRelationship;
using Agecanonix.Domain.Entities;
using Mapster;

namespace Agecanonix.Application.Mappings;

public static class MappingConfig
{
    public static void RegisterMappings()
    {
        // Mapster utilise des conventions par défaut et ne nécessite généralement pas de configuration explicite
        // Les mappings simples sont automatiques

        // Configuration personnalisée si nécessaire
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
    }
}
