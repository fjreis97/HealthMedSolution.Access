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
public class DoctorByServiceController(IDoctorByServiceService _service) : ControllerBase
{
    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<DoctorByServiceResponse?>?>> Create([FromBody] DoctorByServiceRequest request)
    {
        var resp = await _service.Create(request);

        if (resp!.IsSuccess)
            return Created();

        return StatusCode(resp.Code, resp);
    }

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<DoctorByServiceResponse?>?>> Put([FromBody] DoctorByServiceRequest request)
    {
        var resp = await _service.Update(request);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpDelete("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<DoctorByServiceResponse?>?>> Delete(long id)
    {
        var resp = await _service.Delete(id);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<DoctorByServiceResponse?>?>> GetById(long id)
    {
        var resp = await _service.GetById(id);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<PagedResponse<IEnumerable<DoctorByServiceResponse>?>>> GetAll()
    {
        var resp = await _service.GetAllAsync();

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<PagedResponse<IEnumerable<DoctorByServiceResponse>?>>> GetByFilter([FromQuery] SearchDoctorByServiceRequest request)
    {
        var resp = await _service.GetByFilterAsync(request);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }
}
