using AutoMapper;
using RealEstateAgencyApp.Contracts.Dtos.RealEstateObjectDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.Application.Services;

/// <summary>
/// Service for managing real estate object entities.
/// Provides CRUD operations for real estate objects with business logic validation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="RealEstateObjectService"/> class.
/// </remarks>
/// <param name="realEstateRepository">The repository for real estate object data access.</param>
/// <param name="mapper">The mapper for DTO and entity transformations.</param>
public class RealEstateObjectService(IRealEstateObjectRepository realEstateRepository, IMapper mapper) : ICrudService<RealEstateObjectGetDto, RealEstateObjectEditDto>
{
    private readonly IRealEstateObjectRepository _realEstateRepository = realEstateRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Retrieves all real estate objects from the system.
    /// </summary>
    /// <returns>A list of all real estate objects as DTOs.</returns>
    public async Task<List<RealEstateObjectGetDto>> GetAllAsync()
    {
        var estates = await _realEstateRepository.GetAllAsync();
        return _mapper.Map<List<RealEstateObjectGetDto>>(estates);
    }

    /// <summary>
    /// Retrieves a specific real estate object by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the real estate object.</param>
    /// <returns>The real estate object DTO if found; otherwise, null.</returns>
    public async Task<RealEstateObjectGetDto?> GetByIdAsync(int id)
    {
        var estate = await _realEstateRepository.GetByIdAsync(id);
        return _mapper.Map<RealEstateObjectGetDto?>(estate);
    }

    /// <summary>
    /// Creates a new real estate object in the system.
    /// </summary>
    /// <param name="createDto">The data for creating a new real estate object.</param>
    /// <returns>The created real estate object as a DTO.</returns>
    /// <exception cref="InvalidOperationException">Thrown when a real estate object with the same cadastral number already exists.</exception>
    public async Task<RealEstateObjectGetDto> CreateAsync(RealEstateObjectEditDto createDto)
    {
        var existingEstate = await _realEstateRepository.GetByCadastralNumberAsync(createDto.CadastralNumber);
        if (existingEstate != null)
            throw new InvalidOperationException("Real estate object with this cadastral number already exists");

        var newEstate = _mapper.Map<RealEstateObject>(createDto);
        await _realEstateRepository.AddAsync(newEstate);

        return _mapper.Map<RealEstateObjectGetDto>(newEstate);
    }

    /// <summary>
    /// Updates an existing real estate object.
    /// </summary>
    /// <param name="id">The unique identifier of the real estate object to update.</param>
    /// <param name="updateDto">The updated real estate object data.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the real estate object with specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the cadastral number is already taken by another real estate object.</exception>
    public async Task UpdateAsync(int id, RealEstateObjectEditDto updateDto)
    {
        if (!await _realEstateRepository.ExistsByIdAsync(id))
            throw new KeyNotFoundException($"Real estate object with ID {id} not found");

        var existingEstate = await _realEstateRepository.GetByCadastralNumberAsync(updateDto.CadastralNumber);
        if (existingEstate != null && existingEstate.Id != id)
            throw new InvalidOperationException("Real estate object with this cadastral number already exists");

        var updatedEstate = _mapper.Map<RealEstateObject>(updateDto);
        updatedEstate.Id = id;
        await _realEstateRepository.UpdateAsync(updatedEstate);
    }

    /// <summary>
    /// Deletes a real estate object from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the real estate object to delete.</param>
    /// <exception cref="KeyNotFoundException">Thrown when the real estate object with specified ID is not found.</exception>
    public async Task DeleteAsync(int id)
    {
        var isExists = await _realEstateRepository.ExistsByIdAsync(id);
        if (!isExists)
            throw new KeyNotFoundException($"Real estate object with ID {id} not found");

        await _realEstateRepository.DeleteAsync(id);
    }
}