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

public class MedicalScheduleService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors) : IMedicalScheduleService
{

    public async Task<Response<IEnumerable<MedicalScheduleRequest?>?>> GenerateMedicalSchedule(int daysAhead = 30)
    {
        throw new NotImplementedException();
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
}
