using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController(IAppointmentService _AppointmentService) : Controller
{
    [HttpPost("[action]")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> Create([FromBody] AppointmentRequest request)
       => await _AppointmentService.Create(request);

    [HttpPut("[action]")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> Put([FromBody] AppointmentRequest request)
       => await _AppointmentService.Update(request);


    [HttpDelete("[action]/{id}")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> Delete(long id)
       => await _AppointmentService.Delete(id);


    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<Response<AppointmentResponse?>?>> GetById(long id)
      => await _AppointmentService.GetById(id);

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<AppointmentResponse>?>>> GetAll()
        => await _AppointmentService.GetAllAsync();

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<AppointmentResponse>?>>> GetByFilter([FromQuery] SearchAppointmentRequest request)
        => await _AppointmentService.GetByFilterAsync(request);
}
