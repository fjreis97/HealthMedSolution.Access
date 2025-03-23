using Health_Med.Business;
using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalEspecialtyController(IMedicalEspecialtyService _service) : ControllerBase
{
    [HttpPost("[action]")]
    public async Task<ActionResult<Response<MedicalEspecialtyResponse?>?>> Create([FromBody] MedicalEspecialtyRequest request)
         => await _service.Create(request);

    [HttpPut("[action]")]
    public async Task<ActionResult<Response<MedicalEspecialtyResponse?>?>> Put([FromBody] MedicalEspecialtyRequest request)
       => await _service.Update(request);


    [HttpDelete("[action]/{id}")]
    public async Task<ActionResult<Response<MedicalEspecialtyResponse?>?>> Delete(long id)
       => await _service.Delete(id);


    [HttpGet("[action]/{id}")]
    public async Task<ActionResult<Response<MedicalEspecialtyResponse?>?>> GetById(long id)
      => await _service.GetById(id);

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<MedicalEspecialtyResponse>?>>> GetAll()
        => await _service.GetAllAsync();

    [HttpGet("[action]")]
    public async Task<ActionResult<PagedResponse<IEnumerable<MedicalEspecialtyResponse>?>>> GetByFilter([FromBody] SearchMedicalEspecialtyRequest request)
        => await _service.GetByFilterAsync(request);
}
