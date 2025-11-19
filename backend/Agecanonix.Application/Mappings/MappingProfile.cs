using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.DTOs.Resident;
using Agecanonix.Application.DTOs.Contact;
using Agecanonix.Application.DTOs.Stay;
using Agecanonix.Domain.Entities;
using AutoMapper;

namespace Agecanonix.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Facility mappings
        CreateMap<Facility, FacilityDto>();
        CreateMap<CreateFacilityDto, Facility>();
        CreateMap<UpdateFacilityDto, Facility>();

        // Resident mappings
        CreateMap<Resident, ResidentDto>();
        CreateMap<CreateResidentDto, Resident>();
        CreateMap<UpdateResidentDto, Resident>();

        // Contact mappings
        CreateMap<Contact, ContactDto>();
        CreateMap<CreateContactDto, Contact>();
        CreateMap<UpdateContactDto, Contact>();

        // Stay mappings
        CreateMap<Stay, StayDto>();
        CreateMap<CreateStayDto, Stay>();
        CreateMap<UpdateStayDto, Stay>();
    }
}
