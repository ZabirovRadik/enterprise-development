using AutoMapper;
using RealEstateAgencyApp.Contracts.Dtos.RequestDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.Application.Services;

/// <summary>
/// Service for managing request entities.
/// Provides CRUD operations and query capabilities for requests with business logic validation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RequestService"/> class.
/// </remarks>
/// <param name="requestRepository">The repository for request data access.</param>
/// <param name="counterpartyRepository">The repository for counterparty data access.</param>
/// <param name="realEstateRepository">The repository for real estate object data access.</param>
/// <param name="mapper">The mapper for DTO and entity transformations.</param>
public class RequestService(
    IRequestRepository requestRepository,
    ICounterpartyRepository counterpartyRepository,
    IRealEstateObjectRepository realEstateRepository,
    IMapper mapper) : ICrudService<RequestGetDto, RequestEditDto>, IRequestService
{
    private readonly IRequestRepository _requestRepository = requestRepository;
    private readonly ICounterpartyRepository _counterpartyRepository = counterpartyRepository;
    private readonly IRealEstateObjectRepository _realEstateRepository = realEstateRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Retrieves all requests from the system.
    /// </summary>
    /// <returns>A list of all requests as DTOs.</returns>
    public async Task<List<RequestGetDto>> GetAllAsync()
    {
        var requests = await _requestRepository.GetAllAsync();
        return _mapper.Map<List<RequestGetDto>>(requests);
    }

    /// <summary>
    /// Retrieves a specific request by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the request.</param>
    /// <returns>The request DTO if found; otherwise, null.</returns>
    public async Task<RequestGetDto?> GetByIdAsync(int id)
    {
        var request = await _requestRepository.GetByIdAsync(id);
        return _mapper.Map<RequestGetDto?>(request);
    }

    /// <summary>
    /// Retrieves all requests associated with a specific counterparty.
    /// </summary>
    /// <param name="counterpartyId">The unique identifier of the counterparty.</param>
    /// <returns>A list of requests for the specified counterparty.</returns>
    public async Task<List<RequestGetDto>> GetByCounterpartyIdAsync(int counterpartyId)
    {
        var requests = await _requestRepository.GetByCounterpartyIdAsync(counterpartyId);
        return _mapper.Map<List<RequestGetDto>>(requests);
    }

    /// <summary>
    /// Retrieves all requests associated with a specific real estate object.
    /// </summary>
    /// <param name="estateId">The unique identifier of the real estate object.</param>
    /// <returns>A list of requests for the specified real estate object.</returns>
    public async Task<List<RequestGetDto>> GetByEstateIdAsync(int estateId)
    {
        var requests = await _requestRepository.GetByEstateIdAsync(estateId);
        return _mapper.Map<List<RequestGetDto>>(requests);
    }

    /// <summary>
    /// Creates a new request in the system.
    /// </summary>
    /// <param name="createDto">The data for creating a new request.</param>
    /// <returns>The created request as a DTO.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the specified counterparty or real estate object is not found.</exception>
    public async Task<RequestGetDto> CreateAsync(RequestEditDto createDto)
    {
        var isCounterpartyExists = await _counterpartyRepository.ExistsByIdAsync(createDto.CounterpartyID);
        var isEstateExists = await _realEstateRepository.ExistsByIdAsync(createDto.EstateID);

        if (!isCounterpartyExists || !isEstateExists)
            throw new KeyNotFoundException("Counterparty or Real Estate object not found");

        var newRequest = _mapper.Map<Request>(createDto);
        await _requestRepository.AddAsync(newRequest);

        return _mapper.Map<RequestGetDto>(newRequest);
    }

    /// <summary>
    /// Updates an existing request.
    /// </summary>
    /// <param name="id">The unique identifier of the request to update.</param>
    /// <param name="updateDto">The updated request data.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the request with specified ID is not found or when counterparty/real estate object is not found.</exception>
    public async Task UpdateAsync(int id, RequestEditDto updateDto)
    {
        var isCounterpartyExists = await _counterpartyRepository.ExistsByIdAsync(updateDto.CounterpartyID);
        var isEstateExists = await _realEstateRepository.ExistsByIdAsync(updateDto.EstateID);

        if (!isCounterpartyExists || !isEstateExists)
            throw new KeyNotFoundException("Counterparty or Real Estate object not found");
        if (!await _requestRepository.ExistsByIdAsync(id))
            throw new KeyNotFoundException($"Request with ID {id} not found");
        var updatedRequest = _mapper.Map<Request>(updateDto);
        updatedRequest.Id = id;
        await _requestRepository.UpdateAsync(updatedRequest);
    }

    /// <summary>
    /// Deletes a request from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the request to delete.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the request with specified ID is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var isExists = await _requestRepository.ExistsByIdAsync(id);
        if (!isExists)
            throw new KeyNotFoundException($"Request with ID {id} not found");

        await _requestRepository.DeleteAsync(id);
    }
}