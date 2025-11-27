using AutoMapper;
using RealEstateAgencyApp.Contracts.Dtos.AnalyticsDtos;
using RealEstateAgencyApp.Contracts.Dtos.CounterpartyDtos;
using RealEstateAgencyApp.Contracts.Dtos.RealEstateObjectDtos;
using RealEstateAgencyApp.Contracts.Dtos.RequestDtos;
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
        CreateMap<Request, RequestGetDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CounterpartyId, opt => opt.MapFrom(src => src.Counterparty.Id))
            .ForMember(dest => dest.CounterpartyName, opt => opt.MapFrom(src => src.Counterparty.FullName))
            .ForMember(dest => dest.CounterpartyPhone, opt => opt.MapFrom(src => src.Counterparty.Phone))
            .ForMember(dest => dest.EstateId, opt => opt.MapFrom(src => src.Estate.Id))
            .ForMember(dest => dest.EstateAddress, opt => opt.MapFrom(src => src.Estate.Address))
            .ForMember(dest => dest.EstateCadastralNumber, opt => opt.MapFrom(src => src.Estate.CadastralNumber))
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date));

        CreateMap<Counterparty, CounterpartyWithTotalValueDto>();
        CreateMap<RealEstateObject, RealEstateWithRequestCountDto>();
    }
}