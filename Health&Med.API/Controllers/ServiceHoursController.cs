using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceHoursController(IServiceHoursService _service) : ControllerBase
{
    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
    public async Task<ActionResult<Response<ServiceHoursResponse?>?>> Create([FromBody] ServiceHoursRequest request)
      => await _service.Create(request);

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
    public async Task<ActionResult<Response<ServiceHoursResponse?>?>> Put([FromBody] ServiceHoursRequest request)
       => await _service.Update(request);


    [HttpDelete("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
    public async Task<ActionResult<Response<ServiceHoursResponse?>?>> Delete(long id)
       => await _service.Delete(id);


    [HttpGet("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
    public async Task<ActionResult<Response<ServiceHoursResponse?>?>> GetById(long id)
      => await _service.GetById(id);

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
    public async Task<ActionResult<PagedResponse<IEnumerable<ServiceHoursResponse>?>>> GetAll()
        => await _service.GetAllAsync();

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Doctor")]
    public async Task<ActionResult<PagedResponse<IEnumerable<ServiceHoursResponse>?>>> GetByFilter([FromBody] SearchServiceHoursRequest request)
        => await _service.GetByFilterAsync(request);
}