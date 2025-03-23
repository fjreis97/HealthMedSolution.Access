using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorByEspecialtyController(IDoctorByEspecialtyService _service) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<Response<DoctorByEspecialtyResponse?>?>> Create([FromBody] DoctorByEspecialtyRequest request)
         => await _service.Create(request);

    [HttpPut("[action]")]
    public async Task<ActionResult<Response<DoctorByEspecialtyResponse?>?>> Put([FromBody] DoctorByEspecialtyRequest request)
       => await _service.Update(request);


    [HttpDelete("[action]/{id}")]
    public async Task<ActionResult<Response<DoctorByEspecialtyResponse?>?>> Delete(long id)
       => await _service.Delete(id);


    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<Response<DoctorByEspecialtyResponse?>?>> GetById(long id)
      => await _service.GetById(id);

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<DoctorByEspecialtyResponse>?>>> GetAll()
        => await _service.GetAllAsync();

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<DoctorByEspecialtyResponse>?>>> GetByFilter([FromBody] SearchDoctorByEspecialtyRequest request)
        => await _service.GetByFilterAsync(request);
}
