using Health_Med.Business;
using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Enums;
using Health_Med.Infrastructure.Repositories.Interface;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Health_Med.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalScheduleController(IMedicalScheduleService _medicalScheduleService) : ControllerBase
{


    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<IEnumerable<MedicalScheduleRequest?>?>>> GenerateMedicalSchedule(GenerateHoursRequest request)
         => await _medicalScheduleService.GenerateMedicalSchedule(request);

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor, Collaborator")]
    public async Task<ActionResult<Response<bool>>> UpdateAgendaStatus(MedicalScheduleUpdateStatusRequest request)
        => await _medicalScheduleService.UpdateStatusSchedule(request);

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<MedicalScheduleResponse?>?>> Create([FromBody] MedicalScheduleRequest request)
         => await _medicalScheduleService.Create(request);

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<MedicalScheduleResponse?>?>> Put([FromBody] MedicalScheduleRequest request)
       => await _medicalScheduleService.Update(request);


    [HttpDelete("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<MedicalScheduleResponse?>?>> Delete(long id)
       => await _medicalScheduleService.Delete(id);


    [HttpGet("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<MedicalScheduleResponse?>?>> GetById(long id)
      => await _medicalScheduleService.GetById(id);

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<PagedResponse<IEnumerable<MedicalScheduleResponse>?>>> GetAll()
        => await _medicalScheduleService.GetAllAsync();

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor, Patient")]
    public async Task<ActionResult<PagedResponse<IEnumerable<MedicalScheduleResponse>?>>> GetByFilter([FromQuery] SearchMedicalScheduleRequest request)
        => await _medicalScheduleService.GetByFilterAsync(request);

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<bool>>> ApprovedSchedule([FromQuery] long Id)
       => await _medicalScheduleService.ConfirmedSchedule(Id);

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<bool>>> RejectedSchedule([FromQuery] long Id)
       => await _medicalScheduleService.RejectedSchedule(Id);

    //[HttpPut("[action]")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    //public async Task<ActionResult<Response<bool>>> CancellationSchedule([FromQuery] CancellationMedicalScheduleRequest)
    //   => await _medicalScheduleService.CancellationSchedule(Id);
}
