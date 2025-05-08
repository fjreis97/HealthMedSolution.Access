using AutoMapper;
using Health_Med.Business.Interfaces;
using Health_Med.Business.Validators;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Enums;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using HealthMed.API.Access.Common.ResponseDefault;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business;

public class AppointmentService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors, IConfiguration _config) : IAppointmentService
{
    private async Task<IEnumerable<MedicalScheduleResponse?>> GetMedicalScheduleAvailable(AppointmentRequest request)
    {
        var ParametersRequest = new SearchMedicalScheduleRequest
        {
            DoctorId = request.DoctorId,
            SpecialtyId = request.SpecialtyId,
            Date = request.RequestedDate,
            StartTime = request.RequestedTime,
            Status = "Available"
        };

         var response = await _uow.MedicalScheduleRepository.GetByFilterAsync(ParametersRequest);

        if (!response.Any())
        {
            _coletorErrors.AddError("not found Schedule for Appointment");
            return null;
        }

        return _mapper.Map<IEnumerable<MedicalScheduleResponse?>>(response);
    }

    private async Task<bool> IncludeAppointment(MedicalScheduleRequest request, long AppointmentId)
    {

        try
        {

            request.AppointmentId = (int)AppointmentId;  
            request.Status = "Requested";

            var response = await _uow.MedicalScheduleRepository.UpdateAsync(request);

            if (!response)
            {
                _coletorErrors.AddError("Appointment not found");
                return false;
            }

            return true;
        }
        catch
        {
            _coletorErrors.AddError("Error of conection");
            return false;
        }
    }

    public async Task<Response<AppointmentResponse?>?> Create(AppointmentRequest request)
    {
        if (!await Validate(request))
            return new Response<AppointmentResponse?>(null, 404, "Error Creating Appointment", _coletorErrors.GenerateErrors());
        try
        {
            _uow.Begin();
            var medicalSchedule = (await GetMedicalScheduleAvailable(request))?.FirstOrDefault();

            if (medicalSchedule == null)
            {
                _uow.Rollback();
                _coletorErrors.AddError("Medical Schedule not found");
                return new Response<AppointmentResponse?>(null, 404, "Medical Schedule not found", _coletorErrors.GenerateErrors());
            }

            long newCode = await _uow.AppointmentRepository.InsertAsync(_mapper.Map<AppointmentModel>(request));
            var updateSchedule = await IncludeAppointment(_mapper.Map<MedicalScheduleRequest>(medicalSchedule), newCode);

            if (newCode == 0 || updateSchedule == false)
            {
                _uow.Rollback();
                _coletorErrors.AddError("Error Creating Appointment");
                return new Response<AppointmentResponse?>(null, 404, "Error Creating Appointment", _coletorErrors.GenerateErrors());
            }

            _uow.Commit();
            return new Response<AppointmentResponse?>(null, 204, "Appointment Created", null);
        }
        catch (Exception ex)
        {
            _uow.Rollback();
            _coletorErrors.AddError("Error Creating Appointment");
            return new Response<AppointmentResponse?>(null, 404, "Error Creating Appointment", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<AppointmentResponse?>?> Delete(long id)
    {
        var Appointment = await _uow.AppointmentRepository.GetByIdAsync(id);

        if (Appointment == null)
        {
            _coletorErrors.AddError("Appointment not found");
            return new Response<AppointmentResponse?>(null, 404, "Appointment not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.AppointmentRepository.DisableAsync(Appointment);
            return new Response<AppointmentResponse?>(null, 200, "Appointment Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting Appointment");
            return new Response<AppointmentResponse?>(null, 404, "Error Deleting Appointment", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<AppointmentResponse>?>> GetAllAsync()
    {
        var Appointments = await _uow.AppointmentRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<AppointmentResponse>>(Appointments);
        var count = result.Count();

        return new PagedResponse<IEnumerable<AppointmentResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<AppointmentResponse>?>> GetByFilterAsync(SearchAppointmentRequest request)
    {
        var Appointments = await _uow.AppointmentRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<AppointmentResponse>>(Appointments);
        var count = result.Count();

        return new PagedResponse<IEnumerable<AppointmentResponse>?>(result, count);
    }

    public async Task<Response<AppointmentResponse?>> GetById(long id)
    {
        var Appointment = await _uow.AppointmentRepository.GetByIdAsync(id);

        if (Appointment == null)
        {
            _coletorErrors.AddError("Appointment not found");
            return new Response<AppointmentResponse?>(null, 404, "Appointment not found", _coletorErrors.GenerateErrors());
        }

        return new Response<AppointmentResponse?>(_mapper.Map<AppointmentResponse>(Appointment), 200, "Appointment Found", null);
    }


    public async Task<bool> ApprovedAppointment(long id)
    {
        var appointment = await _uow.AppointmentRepository.GetByIdAsync(id);
        appointment.Status = (int)EStatus.Confirmed;
        var response = await _uow.AppointmentRepository.UpdateAsync(appointment);

        if(response)
            return true;

        return false;
    }

    public async Task<bool> RejectedAppointment(long id)
    {
        var appointment = await _uow.AppointmentRepository.GetByIdAsync(id);
        appointment.Status = (int)EStatus.Rejected;
        var response = await _uow.AppointmentRepository.UpdateAsync(appointment);

        if (response)
            return true;

        return false;
    }

    public async Task<Response<AppointmentResponse?>?> ApprovedAppointment(AppointmentRequest request)
    {
        if (!await Validate(request))
            return new Response<AppointmentResponse?>(null, 404, "Error Updating Appointment", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.AppointmentRepository.UpdateAsync(_mapper.Map<AppointmentModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating Appointment");
            return new Response<AppointmentResponse?>(null, 404, "Error Updating Appointment", _coletorErrors.GenerateErrors());
        }

        return new Response<AppointmentResponse?>(null, 200, "Appointment Updated", null);
    }

    public async Task<Response<AppointmentResponse?>?> Update(AppointmentRequest request)
    {
        if (!await Validate(request))
            return new Response<AppointmentResponse?>(null, 404, "Error Updating Appointment", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.AppointmentRepository.UpdateAsync(_mapper.Map<AppointmentModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating Appointment");
            return new Response<AppointmentResponse?>(null, 404, "Error Updating Appointment", _coletorErrors.GenerateErrors());
        }

        return new Response<AppointmentResponse?>(null, 200, "Appointment Updated", null);
    }

    private async Task<bool> Validate(AppointmentRequest request)
    {
        var validator = await new AppointmentValidator().ValidateAsync(request);

        if (!validator.IsValid)
        {

            foreach (var error in validator.Errors)
            {
                _coletorErrors.AddError(error.ErrorMessage);
            }
            return false;
        }
        return true;
    }
}
