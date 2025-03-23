using Health_Med.Business;
using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController(IPatientService _service) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<Response<PatientResponse?>?>> Create([FromBody] PatientRequest request)
        => await _service.Create(request);

    [HttpPut("[action]")]
    public async Task<ActionResult<Response<PatientResponse?>?>> Put([FromBody] PatientRequest request)
       => await _service.Update(request);


    [HttpDelete("[action]/{id}")]
    public async Task<ActionResult<Response<PatientResponse?>?>> Delete(long id)
       => await _service.Delete(id);


    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<Response<PatientResponse?>?>> GetById(long id)
      => await _service.GetById(id);

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<PatientResponse>?>>> GetAll()
        => await _service.GetAllAsync();

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<PatientResponse>?>>> GetByFilter([FromBody] SearchPatientRequest request)
        => await _service.GetByFilterAsync(request);
}
