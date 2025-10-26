using RealEstateAgencyApp.Contracts.Dtos.RequestDtos;

namespace RealEstateAgencyApp.Contracts.Interfaces;

public interface IRequestService : ICrudService<RequestGetDto, RequestEditDto>
{
    public Task<List<RequestGetDto>> GetByCounterpartyIdAsync(int counterpartyId);
    public Task<List<RequestGetDto>> GetByEstateIdAsync(int estateId);
}