using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;
//teste
[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Collaborator,Patient")]
public class AppointmentController(IAppointmentService _AppointmentService) : Controller
{
    [HttpPost("[action]")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> Create([FromBody] AppointmentRequest request)
    {
        var resp = await _AppointmentService.Create(request);

        if(resp!.IsSuccess)
            return Created();

        return StatusCode(resp.Code, resp);
    }


    [HttpPut("[action]")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> Put([FromBody] AppointmentRequest request)
    {
        var resp = await _AppointmentService.Update(request);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp); 
    }

    [HttpDelete("[action]/{id}")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> Delete(long id)
    {
        var resp =  await _AppointmentService.Delete(id);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> GetById(long id)
    {
        var resp =  await _AppointmentService.GetById(id);

        if(resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<AppointmentResponse>?>>> GetAll()
    {
        var resp =  await _AppointmentService.GetAllAsync();

        if(resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<AppointmentResponse>?>>> GetByFilter([FromQuery] SearchAppointmentRequest request)
    {
        var resp = await _AppointmentService.GetByFilterAsync(request);

        if(resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpPut("[action]/{id}")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> CanceledAppointment(long id)
    {
        var resp = await _AppointmentService.CancelAppointment(id);

        if(resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }
}
