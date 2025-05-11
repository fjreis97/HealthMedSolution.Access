using AutoMapper;
using Health_Med.Business.Interfaces;
using Health_Med.Business.Validators;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using HealthMed.API.Access.Common.ResponseDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business;

public class MedicalScheduleService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors, IAppointmentService _appointmentService) : IMedicalScheduleService
{


    public async Task<Response<bool>> ConfirmedSchedule(long id)
    {
        _uow.Begin();
        var schedule = await _uow.MedicalScheduleRepository.GetByIdAsync(id);
        schedule.Status = "Busy";

        var scheduleResponse = await _uow.MedicalScheduleRepository.UpdateAsync(schedule);

        //libera a marcação

        var approvedAppointment = await _appointmentService.ApprovedAppointment((long)schedule.AppointmentId!);

        if (scheduleResponse && approvedAppointment)
        {
            _uow.Commit();
            return new Response<bool>(false, 204, "Confirmed MedicalSchedule", _coletorErrors.GenerateErrors());
        }

        _uow.Rollback();
        return new Response<bool>(false, 404, "Error Confirmed MedicalSchedule", _coletorErrors.GenerateErrors());
    }

    public async Task<Response<bool>> RejectedSchedule(long id)
    {
        _uow.Begin();
        var schedule = await _uow.MedicalScheduleRepository.GetByIdAsync(id);
        var appointmentId = schedule.AppointmentId;
        schedule.Status = "Available";
        schedule.AppointmentId = null;
       
        var scheduleResponse = await _uow.MedicalScheduleRepository.UpdateAsync(schedule);

        //libera a marcação
        var approvedAppointment = await _appointmentService.RejectedAppointment((long)appointmentId!);

        if (scheduleResponse && approvedAppointment)
        {
            _uow.Commit();
            return new Response<bool>(true, 204, "Rejected MedicalSchedule success", _coletorErrors.GenerateErrors());
        }

        _uow.Rollback();
        return new Response<bool>(false, 404, "Error Rejected MedicalSchedule", _coletorErrors.GenerateErrors());
    }

    public async Task<Response<bool>> UpdateStatusSchedule(MedicalScheduleUpdateStatusRequest request)
    {
        if (!await ValidateScheduleBookedStatus(request))
            return new Response<bool>(false, 404, "Error update MedicalSchedule", _coletorErrors.GenerateErrors());

        var medicalSchedule = await _uow.MedicalScheduleRepository.GetByIdAsync(request.IdMedicalSchedule);

        if (medicalSchedule == null)
        {
            _coletorErrors.AddError("MedicalSchedule not found");
            return new Response<bool>(false, 404, "MedicalSchedule not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            medicalSchedule.Status = request.Status;
            medicalSchedule.AppointmentId = request.AppointmentId;

            var IsUpdate = await _uow.MedicalScheduleRepository.UpdateAsync(medicalSchedule);

            if (!IsUpdate)
            {
                _coletorErrors.AddError("Error Updating MedicalSchedule");
                return new Response<bool>(false, 404, "Error Updating MedicalSchedule", _coletorErrors.GenerateErrors());
            }

            return new Response<bool>(true, 200, "MedicalSchedule Updated", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Updating MedicalSchedule");
            return new Response<bool>(false, 404, "Error Updating MedicalSchedule", _coletorErrors.GenerateErrors());
        }

    }

    public async Task<Response<IEnumerable<MedicalScheduleRequest?>?>> GenerateMedicalSchedule(GenerateHoursRequest request)
    {
        var today = DateTime.Today;
        var endDate = today.AddDays(request.daysAhead);

        var availabilities = await _uow.ServiceHoursRepository.GetAllAsync();

        try
        {
            _uow.Begin();
            foreach (var slot in availabilities)
            {
                for (var date = today; date <= endDate; date = date.AddDays(1))
                {
                    if ((int)date.DayOfWeek != slot.DayWeek)
                        continue;

                    // Verifica se já existe um agendamento para o dia e horário
                    var existingSchedule = await _uow.MedicalScheduleRepository.GetByFilterAsync(new SearchMedicalScheduleRequest
                    {
                        DoctorId = slot.DoctorId,
                        Date = date,
                        StartTime = slot.HourInit,
                        SpecialtyId = slot.EspecialtyId
                    });

                    if (existingSchedule.Count() == 0)
                    {
                        MedicalScheduleModel schedule = new MedicalScheduleModel();

                        schedule.DoctorId = slot.DoctorId;
                        schedule.SpecialtyId = slot.EspecialtyId;
                        schedule.Date = date;
                        schedule.StartTime = slot.HourInit;
                        schedule.EndTime = slot.HourEnd;
                        schedule.Status = "Available";


                        var result = await _uow.MedicalScheduleRepository.InsertAsync(schedule);
                    }
                }
            }

            _uow.Commit();
            return new Response<IEnumerable<MedicalScheduleRequest?>?>(null, 200, "Medical Schedule Generated", null);
        }
        catch
        {
            _uow.Rollback();
            _coletorErrors.AddError("Error Generating Medical Schedule");
            return new Response<IEnumerable<MedicalScheduleRequest?>?>(null, 404, "Error Generating Medical Schedule", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<MedicalScheduleResponse?>?> Create(MedicalScheduleRequest request)
    {
        if (!await Validate(request))
            return new Response<MedicalScheduleResponse?>(null, 404, "Error Creating MedicalSchedule", _coletorErrors.GenerateErrors());

        try
        {
            long newCode = await _uow.MedicalScheduleRepository.InsertAsync(_mapper.Map<MedicalScheduleModel>(request));

            if (newCode == 0)
            {
                _coletorErrors.AddError("Error Creating MedicalSchedule");
                return new Response<MedicalScheduleResponse?>(null, 404, "Error Creating MedicalSchedule", _coletorErrors.GenerateErrors());
            }

            return new Response<MedicalScheduleResponse?>(null, 204, "MedicalSchedule Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating MedicalSchedule");
            return new Response<MedicalScheduleResponse?>(null, 404, "Error Creating MedicalSchedule", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<MedicalScheduleResponse?>?> Delete(long id)
    {
        var MedicalSchedule = await _uow.MedicalScheduleRepository.GetByIdAsync(id);

        if (MedicalSchedule == null)
        {
            _coletorErrors.AddError("MedicalSchedule not found");
            return new Response<MedicalScheduleResponse?>(null, 404, "MedicalSchedule not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.MedicalScheduleRepository.DisableAsync(MedicalSchedule);
            return new Response<MedicalScheduleResponse?>(null, 200, "MedicalSchedule Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting MedicalSchedule");
            return new Response<MedicalScheduleResponse?>(null, 404, "Error Deleting MedicalSchedule", _coletorErrors.GenerateErrors());
        }
    }



    public async Task<PagedResponse<IEnumerable<MedicalScheduleResponse>?>> GetAllAsync()
    {
        var MedicalSchedules = await _uow.MedicalScheduleRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<MedicalScheduleResponse>>(MedicalSchedules);
        var count = result.Count();

        return new PagedResponse<IEnumerable<MedicalScheduleResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<MedicalScheduleResponse>?>> GetByFilterAsync(SearchMedicalScheduleRequest request)
    {
        var MedicalSchedules = await _uow.MedicalScheduleRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<MedicalScheduleResponse>>(MedicalSchedules);
        var count = result.Count();

        return new PagedResponse<IEnumerable<MedicalScheduleResponse>?>(result, count);
    }

    public async Task<Response<MedicalScheduleResponse?>> GetById(long id)
    {
        var MedicalSchedule = await _uow.MedicalScheduleRepository.GetByIdAsync(id);

        if (MedicalSchedule == null)
        {
            _coletorErrors.AddError("MedicalSchedule not found");
            return new Response<MedicalScheduleResponse?>(null, 404, "MedicalSchedule not found", _coletorErrors.GenerateErrors());
        }

        return new Response<MedicalScheduleResponse?>(_mapper.Map<MedicalScheduleResponse>(MedicalSchedule), 200, "MedicalSchedule Found", null);
    }

    public async Task<Response<MedicalScheduleResponse?>?> Update(MedicalScheduleRequest request)
    {
        if (!await Validate(request))
            return new Response<MedicalScheduleResponse?>(null, 404, "Error Updating MedicalSchedule", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.MedicalScheduleRepository.UpdateAsync(_mapper.Map<MedicalScheduleModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating MedicalSchedule");
            return new Response<MedicalScheduleResponse?>(null, 404, "Error Updating MedicalSchedule", _coletorErrors.GenerateErrors());
        }

        return new Response<MedicalScheduleResponse?>(null, 200, "MedicalSchedule Updated", null);
    }


    private async Task<bool> Validate(MedicalScheduleRequest request)
    {
        var validator = await new MedicalScheduleValidator().ValidateAsync(request);

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


    private async Task<bool> ValidateScheduleBookedStatus(MedicalScheduleUpdateStatusRequest request)
    {
        var validator = await new MedicalScheduleUpdateValidator().ValidateAsync(request);

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
