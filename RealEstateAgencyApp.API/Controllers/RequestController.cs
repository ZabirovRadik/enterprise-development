using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.Application.Dtos.RequestDtos;
using RealEstateAgencyApp.Domain.Entities;
using RealEstateAgencyApp.Domain.Interfaces;

namespace RealEstateAgencyApp.API.Controllers;

/// <summary>
/// Endpoints for managing requests.
/// </summary>
/// <param name="requestRepository">Repository for accessing request data.</param>
/// <param name="counterpartyRepository">Repository for accessing counterparties.</param>
/// <param name="realEstateRepository">Repository for accessing real estate objects.</param>
/// <param name="mapper">Mapper for DTOs and entities.</param>
[ApiController]
[Route("api/requests")]
public class RequestController(
    IRequestRepository requestRepository,
    ICounterpartyRepository counterpartyRepository,
    IRealEstateObjectRepository realEstateRepository,
    IMapper mapper
) : ControllerBase
{
    /// <summary>
    /// Returns all requests in the system.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<RequestGetDto>>> GetAllRequests()
    {
        var requests = await requestRepository.GetAllAsync();
        var requestsDto = mapper.Map<List<RequestGetDto>>(requests);
        return Ok(requestsDto);
    }

    /// <summary>
    /// Returns a request by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the request to return.</param>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RequestGetDto>> GetRequestById(int id)
    {
        var request = await requestRepository.GetByIdAsync(id);
        if (request == null) return NotFound();

        var requestDto = mapper.Map<RequestGetDto>(request);
        return Ok(requestDto);
    }

    /// <summary>
    /// Returns requests by counterparty ID.
    /// </summary>
    /// <param name="counterpartyId">The ID of the counterparty.</param>
    [HttpGet("by-counterparty/{counterpartyId:int}")]
    public async Task<ActionResult<List<RequestGetDto>>> GetRequestsByCounterparty(int counterpartyId)
    {
        var requests = await requestRepository.GetByCounterpartyIdAsync(counterpartyId);
        var requestsDto = mapper.Map<List<RequestGetDto>>(requests);
        return Ok(requestsDto);
    }

    /// <summary>
    /// Returns requests by real estate object ID.
    /// </summary>
    /// <param name="estateId">The ID of the real estate object.</param>
    [HttpGet("by-estate/{estateId:int}")]
    public async Task<ActionResult<List<RequestGetDto>>> GetRequestsByEstate(int estateId)
    {
        var requests = await requestRepository.GetByEstateIdAsync(estateId);
        var requestsDto = mapper.Map<List<RequestGetDto>>(requests);
        return Ok(requestsDto);
    }

    /// <summary>
    /// Deletes a request by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the request to delete.</param>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteRequestById(int id)
    {
        var isExists = await requestRepository.ExistsByIdAsync(id);
        if (!isExists) return NotFound();

        await requestRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Creates a new request.
    /// </summary>
    /// <param name="newRequestDto">The data for the new request.</param>
    [HttpPost]
    public async Task<ActionResult<RequestGetDto>> CreateRequest([FromBody] RequestEditDto newRequestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Check if counterparty and estate exist
        var isCounterpartyExists = await counterpartyRepository.ExistsByIdAsync(newRequestDto.CounterpartyID);
        var isEstateExists = await realEstateRepository.ExistsByIdAsync(newRequestDto.EstateID);

        if (!isCounterpartyExists || !isEstateExists)
            return NotFound("Counterparty or Real Estate object not found");

        var newRequest = mapper.Map<Request>(newRequestDto);
        await requestRepository.AddAsync(newRequest);

        var resultDto = mapper.Map<RequestGetDto>(newRequest);
        return CreatedAtAction(nameof(GetRequestById), new { id = newRequest.Id }, resultDto);
    }

    /// <summary>
    /// Updates an existing request by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the request to update.</param>
    /// <param name="updatedRequestDto">The updated request data.</param>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateRequest(int id, [FromBody] RequestEditDto updatedRequestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Check if counterparty and estate exist
        var isCounterpartyExists = await counterpartyRepository.ExistsByIdAsync(updatedRequestDto.CounterpartyID);
        var isEstateExists = await realEstateRepository.ExistsByIdAsync(updatedRequestDto.EstateID);

        if (!isCounterpartyExists || !isEstateExists)
            return NotFound("Counterparty or Real Estate object not found");

        var request = await requestRepository.GetByIdAsync(id);
        if (request == null) return NotFound();

        var updatedRequest = mapper.Map<Request>(updatedRequestDto);
        updatedRequest.Id = request.Id;
        await requestRepository.UpdateAsync(updatedRequest);
        return NoContent();
    }
}