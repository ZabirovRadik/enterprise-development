using AutoMapper;
using RealEstateAgencyApp.Application.Dtos.CounterpartyDtos;
using RealEstateAgencyApp.Application.Dtos.RealEstateObjectDtos;
using RealEstateAgencyApp.Application.Dtos.RequestDtos;
using RealEstateAgencyApp.Application.Dtos.AnalyticsDtos;
using RealEstateAgencyApp.Domain.Entities;

namespace RealEstateAgencyApp.Application.Mappers;

/// <summary>
/// AutoMapper profile for mapping between domain entities and their DTOs.
/// </summary>
public class AppMappingProfile : Profile
{
    /// <summary>
    /// Initializes the mappings between DTOs and domain entities.
    /// </summary>
    public AppMappingProfile()
    {
        CreateMap<CounterpartyEditDto, Counterparty>().ReverseMap();
        CreateMap<RealEstateObjectEditDto, RealEstateObject>().ReverseMap();
        CreateMap<RequestEditDto, Request>().ReverseMap();

        CreateMap<Counterparty, CounterpartyGetDto>();
        CreateMap<RealEstateObject, RealEstateObjectGetDto>();
        CreateMap<Request, RequestGetDto>();

        CreateMap<Counterparty, CounterpartyWithTotalValueDto>();
        CreateMap<RealEstateObject, RealEstateWithRequestCountDto>();
    }
}