using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.Contracts.Dtos.RequestDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using System.Net;

namespace RealEstateAgencyApp.Api.Controllers;

/// <summary>
/// Endpoints for managing requests.
/// </summary>
/// <param name="requestService">Service for request operations.</param>
/// <param name="logger">Logger for error logging.</param>
[ApiController]
[Route("api/requests")]
public class RequestController(
    IRequestService requestService,
    ILogger<RequestController> logger
) : ControllerBase
{
    /// <summary>
    /// Returns all requests in the system.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RequestGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<RequestGetDto>>> GetAllRequests()
    {
        try
        {
            var result = await requestService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting all requests");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving requests");
        }
    }

    /// <summary>
    /// Returns a request by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the request to return.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RequestGetDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<RequestGetDto>> GetRequestById(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("GetRequestById called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            var request = await requestService.GetByIdAsync(id);
            if (request == null)
            {
                logger.LogWarning("Request with id {Id} not found", id);
                return NotFound();
            }

            return Ok(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting request by id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the request");
        }
    }

    /// <summary>
    /// Returns requests by counterparty ID.
    /// </summary>
    /// <param name="counterpartyId">The ID of the counterparty.</param>
    [HttpGet("by-counterparty/{counterpartyId:int}")]
    [ProducesResponseType(typeof(List<RequestGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<RequestGetDto>>> GetRequestsByCounterparty(int counterpartyId)
    {
        try
        {
            if (counterpartyId <= 0)
            {
                logger.LogWarning("GetRequestsByCounterparty called with invalid counterpartyId: {CounterpartyId}", counterpartyId);
                return BadRequest("Counterparty ID must be greater than 0");
            }

            var result = await requestService.GetByCounterpartyIdAsync(counterpartyId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting requests by counterparty id: {CounterpartyId}", counterpartyId);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving requests by counterparty");
        }
    }

    /// <summary>
    /// Returns requests by real estate object ID.
    /// </summary>
    /// <param name="estateId">The ID of the real estate object.</param>
    [HttpGet("by-estate/{estateId:int}")]
    [ProducesResponseType(typeof(List<RequestGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<RequestGetDto>>> GetRequestsByEstate(int estateId)
    {
        try
        {
            if (estateId <= 0)
            {
                logger.LogWarning("GetRequestsByEstate called with invalid estateId: {EstateId}", estateId);
                return BadRequest("Estate ID must be greater than 0");
            }

            var result = await requestService.GetByEstateIdAsync(estateId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting requests by estate id: {EstateId}", estateId);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving requests by estate");
        }
    }

    /// <summary>
    /// Deletes a request by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the request to delete.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> DeleteRequestById(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("DeleteRequestById called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            await requestService.DeleteAsync(id);
            logger.LogInformation("Request with id {Id} deleted successfully", id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Attempt to delete non-existent request with id: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting request with id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the request");
        }
    }

    /// <summary>
    /// Creates a new request.
    /// </summary>
    /// <param name="newRequestDto">The data for the new request.</param>
    [HttpPost]
    [ProducesResponseType(typeof(RequestGetDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<RequestGetDto>> CreateRequest([FromBody] RequestEditDto newRequestDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("CreateRequest called with invalid model state: {Errors}",
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var resultDto = await requestService.CreateAsync(newRequestDto);
            logger.LogInformation("Request created successfully with id: {Id}", resultDto.Id);
            return CreatedAtAction(nameof(GetRequestById), new { id = resultDto.Id }, resultDto);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Attempt to create request with non-existent counterparty or estate");
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating request");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the request");
        }
    }

    /// <summary>
    /// Updates an existing request by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the request to update.</param>
    /// <param name="updatedRequestDto">The updated request data.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> UpdateRequest(int id, [FromBody] RequestEditDto updatedRequestDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("UpdateRequest called with invalid model state for id {Id}: {Errors}",
                    id, string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                logger.LogWarning("UpdateRequest called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            await requestService.UpdateAsync(id, updatedRequestDto);
            logger.LogInformation("Request with id {Id} updated successfully", id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Attempt to update non-existent request or related entities with id: {Id}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating request with id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the request");
        }
    }
}