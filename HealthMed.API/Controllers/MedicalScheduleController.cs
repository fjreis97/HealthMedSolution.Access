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
    {
        var resp = await _medicalScheduleService.GenerateMedicalSchedule(request);

        if(resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor, Collaborator")]
    public async Task<ActionResult<Response<bool>>> UpdateAgendaStatus(MedicalScheduleUpdateStatusRequest request)
    {
        var resp = await _medicalScheduleService.UpdateStatusSchedule(request);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpPost("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<MedicalScheduleResponse?>?>> Create([FromBody] MedicalScheduleRequest request)
    {
        var resp = await _medicalScheduleService.Create(request);

        if (resp!.IsSuccess)
            return Created();

        return StatusCode(resp.Code, resp);
    }

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<MedicalScheduleResponse?>?>> Put([FromBody] MedicalScheduleRequest request)
    {
        var resp = await _medicalScheduleService.Update(request);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);

    }

    [HttpDelete("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<MedicalScheduleResponse?>?>> Delete(long id)
    {
        var resp = await _medicalScheduleService.Delete(id);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]/{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<MedicalScheduleResponse?>?>> GetById(long id)
    {
        var resp = await _medicalScheduleService.GetById(id);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<PagedResponse<IEnumerable<MedicalScheduleResponse>?>>> GetAll()
    {
        var resp = await _medicalScheduleService.GetAllAsync();

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpGet("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor, Patient")]
    public async Task<ActionResult<PagedResponse<IEnumerable<MedicalScheduleResponse>?>>> GetByFilter([FromQuery] SearchMedicalScheduleRequest request)
    {
        var resp = await _medicalScheduleService.GetByFilterAsync(request);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<bool>>> ApprovedSchedule([FromQuery] long Id)
    {
        var resp = await _medicalScheduleService.ConfirmedSchedule(Id);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }

    [HttpPut("[action]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin, Doctor")]
    public async Task<ActionResult<Response<bool>>> RejectedSchedule([FromQuery] long Id)
    {
        var resp = await _medicalScheduleService.RejectedSchedule(Id);

        if (resp.IsSuccess)
            return Ok(resp);

        return StatusCode(resp.Code, resp);
    }
}
