using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.Application.Dtos.RealEstateObjectDtos;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.API.Controllers;

/// <summary>
/// Endpoints for managing real estate objects.
/// </summary>
/// <param name="realEstateRepository">Repository for accessing real estate object data.</param>
/// <param name="mapper">Mapper for DTOs and entities.</param>
[ApiController]
[Route("api/real-estate-objects")]
public class RealEstateObjectController(
    IRealEstateObjectRepository realEstateRepository,
    IMapper mapper
) : ControllerBase
{
    /// <summary>
    /// Returns all real estate objects in the system.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<RealEstateObjectGetDto>>> GetAllRealEstateObjects()
    {
        var estates = await realEstateRepository.GetAllAsync();
        var estatesDto = mapper.Map<List<RealEstateObjectGetDto>>(estates);
        return Ok(estatesDto);
    }

    /// <summary>
    /// Returns a real estate object by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the real estate object to return.</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RealEstateObjectGetDto>> GetRealEstateObjectById(int id)
    {
        var estate = await realEstateRepository.GetByIdAsync(id);
        if (estate == null) return NotFound();

        var estateDto = mapper.Map<RealEstateObjectGetDto>(estate);
        return Ok(estateDto);
    }

    /// <summary>
    /// Deletes a real estate object by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the real estate object to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRealEstateObjectById(int id)
    {
        var isExists = await realEstateRepository.ExistsByIdAsync(id);
        if (!isExists) return NotFound();

        await realEstateRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Creates a new real estate object.
    /// </summary>
    /// <param name="newEstateDto">The data of the real estate object to create.</param>
    [HttpPost]
    public async Task<ActionResult<RealEstateObjectGetDto>> CreateRealEstateObject([FromBody] RealEstateObjectEditDto newEstateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Check if cadastral number already exists
        var existingEstate = await realEstateRepository.GetByCadastralNumberAsync(newEstateDto.CadastralNumber);
        if (existingEstate != null)
            return BadRequest("Real estate object with this cadastral number already exists");

        var newEstate = mapper.Map<RealEstateObject>(newEstateDto);
        await realEstateRepository.AddAsync(newEstate);

        var resultDto = mapper.Map<RealEstateObjectGetDto>(newEstate);
        return CreatedAtAction(nameof(GetRealEstateObjectById), new { id = newEstate.Id }, resultDto);
    }

    /// <summary>
    /// Updates an existing real estate object by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the real estate object to update.</param>
    /// <param name="updatedEstateDto">The updated real estate object data.</param>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateRealEstateObject(int id, [FromBody] RealEstateObjectEditDto updatedEstateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var estate = await realEstateRepository.GetByIdAsync(id);
        if (estate == null) return NotFound();

        // Check if cadastral number is taken by another estate
        var existingEstate = await realEstateRepository.GetByCadastralNumberAsync(updatedEstateDto.CadastralNumber);
        if (existingEstate != null && existingEstate.Id != id)
            return BadRequest("Real estate object with this cadastral number already exists");

        var updatedEstate = mapper.Map<RealEstateObject>(updatedEstateDto);
        updatedEstate.Id = estate.Id;
        await realEstateRepository.UpdateAsync(updatedEstate);
        return NoContent();
    }
}