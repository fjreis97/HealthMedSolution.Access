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

public class DoctorByServiceService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors) : IDoctorByServiceService
{
    public async Task<Response<DoctorByServiceResponse?>> Create(DoctorByServiceRequest request)
    {
        if (!await Validate(request))
            return new Response<DoctorByServiceResponse?>(null, 404, "Error Creating DoctorByService", _coletorErrors.GenerateErrors());

        try
        {
            long newCode = await _uow.DoctorByServiceRepository.InsertAsync(_mapper.Map<DoctorByServiceModel>(request));
            return new Response<DoctorByServiceResponse?>(null, 200, "DoctorByService Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating DoctorByService");
            return new Response<DoctorByServiceResponse?>(null, 404, "Error Creating DoctorByService", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<DoctorByServiceResponse>?> Delete(long id)
    {
        var DoctorByService = await _uow.DoctorByServiceRepository.GetByIdAsync(id);

        if (DoctorByService == null)
        {
            _coletorErrors.AddError("DoctorByService not found");
            return new Response<DoctorByServiceResponse?>(null, 404, "DoctorByService not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.DoctorByServiceRepository.DisableAsync(DoctorByService);
            return new Response<DoctorByServiceResponse?>(null, 200, "DoctorByService Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting DoctorByService");
            return new Response<DoctorByServiceResponse?>(null, 404, "Error Deleting DoctorByService", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<DoctorByServiceResponse>>> GetAllAsync()
    {
        var DoctorByServices = await _uow.DoctorByServiceRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<DoctorByServiceResponse>>(DoctorByServices);
        var count = result.Count();

        return new PagedResponse<IEnumerable<DoctorByServiceResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<DoctorByServiceResponse>>> GetByFilterAsync(SearchDoctorByServiceRequest request)
    {
        var DoctorByServices = await _uow.DoctorByServiceRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<DoctorByServiceResponse>>(DoctorByServices);
        var count = result.Count();

        return new PagedResponse<IEnumerable<DoctorByServiceResponse>?>(result, count);
    }

    public async Task<Response<DoctorByServiceResponse>> GetById(long id)
    {
        var DoctorByService = await _uow.DoctorByServiceRepository.GetByIdAsync(id);

        if (DoctorByService == null)
        {
            _coletorErrors.AddError("DoctorByService not found");
            return new Response<DoctorByServiceResponse?>(null, 404, "DoctorByService not found", _coletorErrors.GenerateErrors());
        }

        return new Response<DoctorByServiceResponse?>(_mapper.Map<DoctorByServiceResponse>(DoctorByService), 200, "DoctorByService Found", null);
    }

    public async Task<Response<DoctorByServiceResponse>?> Update(DoctorByServiceRequest request)
    {
        if (!await Validate(request))
            return new Response<DoctorByServiceResponse?>(null, 404, "Error Updating DoctorByService", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.DoctorByServiceRepository.UpdateAsync(_mapper.Map<DoctorByServiceModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating DoctorByService");
            return new Response<DoctorByServiceResponse?>(null, 404, "Error Updating DoctorByService", _coletorErrors.GenerateErrors());
        }

        return new Response<DoctorByServiceResponse?>(null, 200, "DoctorByService Updated", null);
    }

    private async Task<bool> Validate(DoctorByServiceRequest request)
    {
        var Validate = await new DoctorByServiceValidator().ValidateAsync(request);

        if (!Validate.IsValid)
        {

            foreach (var error in Validate.Errors)
            {
                _coletorErrors.AddError(error.ErrorMessage);
            }
            return false;
        }
        return true;
    }
}
