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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business;

//Horarios de atendimento


public class ServiceHoursService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors, IDoctorService _doctorService, IPasswordGenerate _hash) : IServiceHoursService
{
    public async Task<Response<ServiceHoursResponse?>?> Create(ServiceHoursRequest request)
    {

        if (!await Validate(request))
            return new Response<ServiceHoursResponse?>(null, 404, "Error Creating Service Hours", _coletorErrors.GenerateErrors());

        try
        {

            var ListHourByDoctor = (await _uow.ServiceHoursRepository.GetAllAsync()).Where(x => x.DoctorId == request.DoctorId);

            //TODO: Implementar essa funcionalidade
            //if(ListHourByDoctor.Any())
            //{
            //    ListHourByDoctor.Where(x => x.HourInit == request.HourInit && x.HourEnd == request.HourEnd).ToList().ForEach(x =>
            //    {
            //        _coletorErrors.AddError($"Service Hours already registered for this doctor in the period {x.HourInit} - {x.HourEnd}");                
            //    });
            //    return new Response<ServiceHoursResponse?>(null, 404, "Error Creating Service Hours", _coletorErrors.GenerateErrors());
            //}
            request.NormalizeHours();
            long newCode = await _uow.ServiceHoursRepository.InsertAsync(_mapper.Map<ServiceHoursModel>(request));

            if (newCode == 0)
            {
                _coletorErrors.AddError("Error Creating Service Hours");
                return new Response<ServiceHoursResponse?>(null, 404, "Error Creating Service Hours", _coletorErrors.GenerateErrors());
            }

            return new Response<ServiceHoursResponse?>(null, 204, "Service Hours Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating Service Hours");
            return new Response<ServiceHoursResponse?>(null, 404, "Error Creating Service Hours", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<ServiceHoursResponse?>?> Delete(long id)
    {
        var responseRepository = await _uow.ServiceHoursRepository.GetByIdAsync(id);

        if (responseRepository == null)
        {
            _coletorErrors.AddError("Service Hours not found");
            return new Response<ServiceHoursResponse?>(null, 404, "Service Hours not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.ServiceHoursRepository.DisableAsync(responseRepository);
            return new Response<ServiceHoursResponse?>(null, 200, "Service Hours Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting Service Hours");
            return new Response<ServiceHoursResponse?>(null, 404, "Error Deleting Service Hours", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<ServiceHoursResponse>?>> GetAllAsync()
    {
        var ResponseRepository = await _uow.ServiceHoursRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<ServiceHoursResponse>>(ResponseRepository);
        var count = result.Count();

        return new PagedResponse<IEnumerable<ServiceHoursResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<ServiceHoursResponse>?>> GetByFilterAsync(SearchServiceHoursRequest request)
    {
        var ResponseRepository = await _uow.ServiceHoursRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<ServiceHoursResponse>>(ResponseRepository);
        var count = result.Count();

        return new PagedResponse<IEnumerable<ServiceHoursResponse>?>(result, count);
    }

    public async Task<Response<ServiceHoursResponse?>> GetById(long id)
    {
        var ResponseRepository = await _uow.ServiceHoursRepository.GetByIdAsync(id);

        if (ResponseRepository == null)
        {
            _coletorErrors.AddError("Service Hours not found");
            return new Response<ServiceHoursResponse?>(null, 404, "Service Hours not found", _coletorErrors.GenerateErrors());
        }

        return new Response<ServiceHoursResponse?>(_mapper.Map<ServiceHoursResponse>(ResponseRepository), 200, "Service Hours Found", null);
    }

    public async Task<Response<ServiceHoursResponse?>?> Update(ServiceHoursRequest request)
    {
        if (!await Validate(request))
            return new Response<ServiceHoursResponse?>(null, 404, "Error Updating Service Hours", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.ServiceHoursRepository.UpdateAsync(_mapper.Map<ServiceHoursModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating Service Hours");
            return new Response<ServiceHoursResponse?>(null, 404, "Error Updating Service Hours", _coletorErrors.GenerateErrors());
        }

        return new Response<ServiceHoursResponse?>(null, 200, "Service Hours Updated", null);
    }

   

    private async Task<bool> Validate(ServiceHoursRequest request)
    {
        var validator = await new ServiceHoursValidator().ValidateAsync(request);

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
