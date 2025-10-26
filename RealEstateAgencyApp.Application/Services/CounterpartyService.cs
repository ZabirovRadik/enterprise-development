using AutoMapper;
using RealEstateAgencyApp.Contracts.Dtos.CounterpartyDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.Application.Services;

/// <summary>
/// Service for managing counterparty entities.
/// Provides CRUD operations for counterparties with business logic validation.
/// </summary>
public class CounterpartyService : ICrudService<CounterpartyGetDto, CounterpartyEditDto>
{
    private readonly ICounterpartyRepository _counterpartyRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CounterpartyService"/> class.
    /// </summary>
    /// <param name="counterpartyRepository">The repository for counterparty data access.</param>
    /// <param name="mapper">The mapper for DTO and entity transformations.</param>
    public CounterpartyService(ICounterpartyRepository counterpartyRepository, IMapper mapper)
    {
        _counterpartyRepository = counterpartyRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves all counterparties from the system.
    /// </summary>
    /// <returns>A list of all counterparties as DTOs.</returns>
    public async Task<List<CounterpartyGetDto>> GetAllAsync()
    {
        var counterparties = await _counterpartyRepository.GetAllAsync();
        return _mapper.Map<List<CounterpartyGetDto>>(counterparties);
    }

    /// <summary>
    /// Retrieves a specific counterparty by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the counterparty.</param>
    /// <returns>The counterparty DTO if found; otherwise, null.</returns>
    public async Task<CounterpartyGetDto?> GetByIdAsync(int id)
    {
        var counterparty = await _counterpartyRepository.GetByIdAsync(id);
        return _mapper.Map<CounterpartyGetDto?>(counterparty);
    }

    /// <summary>
    /// Creates a new counterparty in the system.
    /// </summary>
    /// <param name="createDto">The data for creating a new counterparty.</param>
    /// <returns>The created counterparty as a DTO.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a counterparty with the same passport number already exists.</exception>
    public async Task<CounterpartyGetDto> CreateAsync(CounterpartyEditDto createDto)
    {
        // Check if passport number already exists
        var existingCounterparty = await _counterpartyRepository.GetByPassportNumberAsync(createDto.PassportNumber);
        if (existingCounterparty != null)
            throw new InvalidOperationException("Counterparty with this passport number already exists");

        var newCounterparty = _mapper.Map<Counterparty>(createDto);
        await _counterpartyRepository.AddAsync(newCounterparty);

        return _mapper.Map<CounterpartyGetDto>(newCounterparty);
    }

    /// <summary>
    /// Updates an existing counterparty.
    /// </summary>
    /// <param name="id">The unique identifier of the counterparty to update.</param>
    /// <param name="updateDto">The updated counterparty data.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the counterparty with specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the passport number is already taken by another counterparty.</exception>
    public async Task UpdateAsync(int id, CounterpartyEditDto updateDto)
    {
        var counterparty = await _counterpartyRepository.GetByIdAsync(id);
        if (counterparty == null)
            throw new KeyNotFoundException($"Counterparty with ID {id} not found");

        // Check if passport number is taken by another counterparty
        var existingCounterparty = await _counterpartyRepository.GetByPassportNumberAsync(updateDto.PassportNumber);
        if (existingCounterparty != null && existingCounterparty.Id != id)
            throw new InvalidOperationException("Counterparty with this passport number already exists");

        var updatedCounterparty = _mapper.Map<Counterparty>(updateDto);
        updatedCounterparty.Id = id;
        await _counterpartyRepository.UpdateAsync(updatedCounterparty);
    }

    /// <summary>
    /// Deletes a counterparty from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the counterparty to delete.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the counterparty with specified ID is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var isExists = await _counterpartyRepository.ExistsByIdAsync(id);
        if (!isExists)
            throw new KeyNotFoundException($"Counterparty with ID {id} not found");

        await _counterpartyRepository.DeleteAsync(id);
    }
}