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
public class MedicalServiceController(IMedicalServiceService _service) : ControllerBase
{
    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<MedicalServiceResponse?>?>> Create([FromBody] MedicalServiceRequest request)
       => await _service.Create(request);

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<MedicalServiceResponse?>?>> Put([FromBody] MedicalServiceRequest request)
       => await _service.Update(request);


    [HttpDelete("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<MedicalServiceResponse?>?>> Delete(long id)
       => await _service.Delete(id);


    [HttpGet("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<MedicalServiceResponse?>?>> GetById(long id)
      => await _service.GetById(id);

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<MedicalServiceResponse>?>>> GetAll()
        => await _service.GetAllAsync();

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<MedicalServiceResponse>?>>> GetByFilter([FromBody] SearchMedicalServiceRequest request)
        => await _service.GetByFilterAsync(request);
}
