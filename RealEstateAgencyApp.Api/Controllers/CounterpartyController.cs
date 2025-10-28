using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.Contracts.Dtos.CounterpartyDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using System.Net;

namespace RealEstateAgencyApp.Api.Controllers;

/// <summary>
/// Endpoints for managing counterparties.
/// </summary>
/// <param name="counterpartyService">Service for counterparty operations.</param>
/// <param name="logger">Logger for error logging.</param>
[ApiController]
[Route("api/counterparties")]
public class CounterpartyController(
    ICrudService<CounterpartyGetDto, CounterpartyEditDto> counterpartyService,
    ILogger<CounterpartyController> logger
) : ControllerBase
{
    /// <summary>
    /// Returns all counterparties in the system.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CounterpartyGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<CounterpartyGetDto>>> GetAllCounterparties()
    {
        try
        {
            var result = await counterpartyService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting all counterparties");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving counterparties");
        }
    }

    /// <summary>
    /// Returns a counterparty by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the counterparty to return.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CounterpartyGetDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<CounterpartyGetDto>> GetCounterpartyById(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("GetCounterpartyById called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            var counterparty = await counterpartyService.GetByIdAsync(id);
            if (counterparty == null)
            {
                logger.LogWarning("Counterparty with id {Id} not found", id);
                return NotFound();
            }

            return Ok(counterparty);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting counterparty by id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the counterparty");
        }
    }

    /// <summary>
    /// Deletes a counterparty by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the counterparty to delete.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> DeleteCounterpartyById(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("DeleteCounterpartyById called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            await counterpartyService.DeleteAsync(id);
            logger.LogInformation("Counterparty with id {Id} deleted successfully", id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Attempt to delete non-existent counterparty with id: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting counterparty with id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the counterparty");
        }
    }

    /// <summary>
    /// Creates a new counterparty.
    /// </summary>
    /// <param name="newCounterpartyDto">The data of the counterparty to create.</param>
    [HttpPost]
    [ProducesResponseType(typeof(CounterpartyGetDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<CounterpartyGetDto>> CreateCounterparty([FromBody] CounterpartyEditDto newCounterpartyDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("CreateCounterparty called with invalid model state: {Errors}",
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var resultDto = await counterpartyService.CreateAsync(newCounterpartyDto);
            logger.LogInformation("Counterparty created successfully with id: {Id}", resultDto.Id);
            return CreatedAtAction(nameof(GetCounterpartyById), new { id = resultDto.Id }, resultDto);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            logger.LogWarning(ex, "Attempt to create counterparty with duplicate passport number");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating counterparty");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the counterparty");
        }
    }

    /// <summary>
    /// Updates an existing counterparty by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the counterparty to update.</param>
    /// <param name="updatedCounterpartyDto">The updated counterparty data.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> UpdateCounterparty(int id, [FromBody] CounterpartyEditDto updatedCounterpartyDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("UpdateCounterparty called with invalid model state for id {Id}: {Errors}",
                    id, string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                logger.LogWarning("UpdateCounterparty called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            await counterpartyService.UpdateAsync(id, updatedCounterpartyDto);
            logger.LogInformation("Counterparty with id {Id} updated successfully", id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Attempt to update non-existent counterparty with id: {Id}", id);
            return NotFound();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            logger.LogWarning(ex, "Attempt to update counterparty with duplicate passport number for id: {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating counterparty with id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the counterparty");
        }
    }
}