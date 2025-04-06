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
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<CollaboratorResponse?>?>> Create([FromBody] CollaboratorRequest request)
        => await _collaboratorService.Create(request);

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<CollaboratorResponse?>?>> Put([FromBody] CollaboratorRequest request)
       => await _collaboratorService.Update(request);


    [HttpDelete("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<CollaboratorResponse?>?>> Delete(long id)
       => await _collaboratorService.Delete(id);


    [HttpGet("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<CollaboratorResponse?>?>> GetById(long id)
      => await _collaboratorService.GetById(id);

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<CollaboratorResponse>?>>> GetAll()
        => await _collaboratorService.GetAllAsync();

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<CollaboratorResponse>?>>> GetByFilter([FromBody] SearchCollaboratorRequest request)
        => await _collaboratorService.GetByFilterAsync(request);
}
