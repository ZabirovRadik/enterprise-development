using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.Application.Dtos.CounterpartyDtos;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.API.Controllers;

/// <summary>
/// Endpoints for managing counterparties.
/// </summary>
/// <param name="counterpartyRepository">Repository for accessing counterparty data.</param>
/// <param name="mapper">Mapper for DTOs and entities.</param>
[ApiController]
[Route("api/counterparties")]
public class CounterpartyController(
    ICounterpartyRepository counterpartyRepository,
    IMapper mapper
) : ControllerBase
{
    /// <summary>
    /// Returns all counterparties in the system.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<CounterpartyGetDto>>> GetAllCounterparties()
    {
        var counterparties = await counterpartyRepository.GetAllAsync();
        var counterpartiesDto = mapper.Map<List<CounterpartyGetDto>>(counterparties);
        return Ok(counterpartiesDto);
    }

    /// <summary>
    /// Returns a counterparty by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the counterparty to return.</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CounterpartyGetDto>> GetCounterpartyById(int id)
    {
        var counterparty = await counterpartyRepository.GetByIdAsync(id);
        if (counterparty == null) return NotFound();

        var counterpartyDto = mapper.Map<CounterpartyGetDto>(counterparty);
        return Ok(counterpartyDto);
    }

    /// <summary>
    /// Deletes a counterparty by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the counterparty to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCounterpartyById(int id)
    {
        var isExists = await counterpartyRepository.ExistsByIdAsync(id);
        if (!isExists) return NotFound();

        await counterpartyRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Creates a new counterparty.
    /// </summary>
    /// <param name="newCounterpartyDto">The data of the counterparty to create.</param>
    [HttpPost]
    public async Task<ActionResult<CounterpartyGetDto>> CreateCounterparty([FromBody] CounterpartyEditDto newCounterpartyDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Check if passport number already exists
        var existingCounterparty = await counterpartyRepository.GetByPassportNumberAsync(newCounterpartyDto.PassportNumber);
        if (existingCounterparty != null)
            return BadRequest("Counterparty with this passport number already exists");

        var newCounterparty = mapper.Map<Counterparty>(newCounterpartyDto);
        await counterpartyRepository.AddAsync(newCounterparty);

        var resultDto = mapper.Map<CounterpartyGetDto>(newCounterparty);
        return CreatedAtAction(nameof(GetCounterpartyById), new { id = newCounterparty.Id }, resultDto);
    }

    /// <summary>
    /// Updates an existing counterparty by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the counterparty to update.</param>
    /// <param name="updatedCounterpartyDto">The updated counterparty data.</param>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateCounterparty(int id, [FromBody] CounterpartyEditDto updatedCounterpartyDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var counterparty = await counterpartyRepository.GetByIdAsync(id);
        if (counterparty == null) return NotFound();

        // Check if passport number is taken by another counterparty
        var existingCounterparty = await counterpartyRepository.GetByPassportNumberAsync(updatedCounterpartyDto.PassportNumber);
        if (existingCounterparty != null && existingCounterparty.Id != id)
            return BadRequest("Counterparty with this passport number already exists");

        var updatedCounterparty = mapper.Map<Counterparty>(updatedCounterpartyDto);
        updatedCounterparty.Id = counterparty.Id;
        await counterpartyRepository.UpdateAsync(updatedCounterparty);
        return NoContent();
    }
}