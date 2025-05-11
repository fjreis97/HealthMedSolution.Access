using Health_Med.Business;
using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollaboratorController(ICollaboratorService _collaboratorService) : ControllerBase
{
    [HttpPost("[action]")]
    [AllowAnonymous]
    public async Task<ActionResult<Response<CollaboratorResponse?>?>> Create([FromBody] CollaboratorRequest request)
    {
        var resp = await _collaboratorService.Create(request);

        if (resp!.IsSuccess)
            return Created();

        return StatusCode(resp.Code, resp);
    }

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<CollaboratorResponse?>?>> Put([FromBody] CollaboratorRequest request)
    {
        var resp = await _collaboratorService.Update(request);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpDelete("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<CollaboratorResponse?>?>> Delete(long id)
    {
        var resp = await _collaboratorService.Delete(id);


        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<CollaboratorResponse?>?>> GetById(long id)
    {
        var resp = await _collaboratorService.GetById(id);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<CollaboratorResponse>?>>> GetAll()
    {
        var resp = await _collaboratorService.GetAllAsync();

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<CollaboratorResponse>?>>> GetByFilter([FromQuery] SearchCollaboratorRequest request)
    {
        var resp = await _collaboratorService.GetByFilterAsync(request);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }
}
