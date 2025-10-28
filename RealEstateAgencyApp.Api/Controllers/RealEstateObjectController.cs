using Microsoft.AspNetCore.Mvc;
using RealEstateAgencyApp.Contracts.Dtos.RealEstateObjectDtos;
using RealEstateAgencyApp.Contracts.Interfaces;
using System.Net;

namespace RealEstateAgencyApp.Api.Controllers;

/// <summary>
/// Endpoints for managing real estate objects.
/// </summary>
/// <param name="realEstateService">Service for real estate object operations.</param>
/// <param name="logger">Logger for error logging.</param>
[ApiController]
[Route("api/real-estate-objects")]
public class RealEstateObjectController(
    ICrudService<RealEstateObjectGetDto, RealEstateObjectEditDto> realEstateService,
    ILogger<RealEstateObjectController> logger
) : ControllerBase
{
    /// <summary>
    /// Returns all real estate objects in the system.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<RealEstateObjectGetDto>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<List<RealEstateObjectGetDto>>> GetAllRealEstateObjects()
    {
        try
        {
            var result = await realEstateService.GetAllAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting all real estate objects");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving real estate objects");
        }
    }

    /// <summary>
    /// Returns a real estate object by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the real estate object to return.</param>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RealEstateObjectGetDto), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<RealEstateObjectGetDto>> GetRealEstateObjectById(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("GetRealEstateObjectById called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            var estate = await realEstateService.GetByIdAsync(id);
            if (estate == null)
            {
                logger.LogWarning("Real estate object with id {Id} not found", id);
                return NotFound();
            }

            return Ok(estate);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting real estate object by id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving the real estate object");
        }
    }

    /// <summary>
    /// Deletes a real estate object by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the real estate object to delete.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> DeleteRealEstateObjectById(int id)
    {
        try
        {
            if (id <= 0)
            {
                logger.LogWarning("DeleteRealEstateObjectById called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            await realEstateService.DeleteAsync(id);
            logger.LogInformation("Real estate object with id {Id} deleted successfully", id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Attempt to delete non-existent real estate object with id: {Id}", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while deleting real estate object with id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while deleting the real estate object");
        }
    }

    /// <summary>
    /// Creates a new real estate object.
    /// </summary>
    /// <param name="newEstateDto">The data of the real estate object to create.</param>
    [HttpPost]
    [ProducesResponseType(typeof(RealEstateObjectGetDto), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult<RealEstateObjectGetDto>> CreateRealEstateObject([FromBody] RealEstateObjectEditDto newEstateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("CreateRealEstateObject called with invalid model state: {Errors}",
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var resultDto = await realEstateService.CreateAsync(newEstateDto);
            logger.LogInformation("Real estate object created successfully with id: {Id}", resultDto.Id);
            return CreatedAtAction(nameof(GetRealEstateObjectById), new { id = resultDto.Id }, resultDto);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            logger.LogWarning(ex, "Attempt to create real estate object with duplicate cadastral number");
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while creating real estate object");
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while creating the real estate object");
        }
    }

    /// <summary>
    /// Updates an existing real estate object by its unique ID.
    /// </summary>
    /// <param name="id">The ID of the real estate object to update.</param>
    /// <param name="updatedEstateDto">The updated real estate object data.</param>
    [HttpPut("{id:int}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.InternalServerError)]
    public async Task<ActionResult> UpdateRealEstateObject(int id, [FromBody] RealEstateObjectEditDto updatedEstateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                logger.LogWarning("UpdateRealEstateObject called with invalid model state for id {Id}: {Errors}",
                    id, string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            if (id <= 0)
            {
                logger.LogWarning("UpdateRealEstateObject called with invalid id: {Id}", id);
                return BadRequest("ID must be greater than 0");
            }

            await realEstateService.UpdateAsync(id, updatedEstateDto);
            logger.LogInformation("Real estate object with id {Id} updated successfully", id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "Attempt to update non-existent real estate object with id: {Id}", id);
            return NotFound();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("already exists"))
        {
            logger.LogWarning(ex, "Attempt to update real estate object with duplicate cadastral number for id: {Id}", id);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while updating real estate object with id: {Id}", id);
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while updating the real estate object");
        }
    }
}