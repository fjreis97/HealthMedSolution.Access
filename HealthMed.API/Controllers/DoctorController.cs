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
public class DoctorController(IDoctorService _service) : ControllerBase
{
    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<DoctorResponse?>?>> Create([FromBody] DoctorRequest request)
          => await _service.Create(request);

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<DoctorResponse?>?>> Put([FromBody] DoctorRequest request)
       => await _service.Update(request);


    [HttpDelete("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<DoctorResponse?>?>> Delete(long id)
       => await _service.Delete(id);


    [HttpGet("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<Response<DoctorResponse?>?>> GetById(long id)
      => await _service.GetById(id);

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<DoctorResponse>?>>> GetAll()
        => await _service.GetAllAsync();

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator")]
    public async Task<ActionResult<PagedResponse<IEnumerable<DoctorResponse>?>>> GetByFilter([FromQuery] SearchDoctorRequest request)
        => await _service.GetByFilterAsync(request);
}
