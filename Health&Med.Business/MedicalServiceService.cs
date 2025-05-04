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

public class MedicalServiceService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors) : IMedicalServiceService
{
    public async Task<Response<MedicalServiceResponse?>?> Create(MedicalServiceRequest request)
    {
        if (!await Validate(request))
            return new Response<MedicalServiceResponse?>(null, 404, "Error Creating MedicalService", _coletorErrors.GenerateErrors());

        try
        {
            long newCode = await _uow.MedicalServiceRepository.InsertAsync(_mapper.Map<MedicalServiceModel>(request));

            if (newCode == 0)
            {
                _coletorErrors.AddError("Error Creating MedicalService");
                return new Response<MedicalServiceResponse?>(null, 404, "Error Creating MedicalService", _coletorErrors.GenerateErrors());
            }

            return new Response<MedicalServiceResponse?>(null, 200, "MedicalService Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating MedicalService");
            return new Response<MedicalServiceResponse?>(null, 404, "Error Creating MedicalService", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<MedicalServiceResponse?>?> Delete(long id)
    {
        var MedicalService = await _uow.MedicalServiceRepository.GetByIdAsync(id);

        if (MedicalService == null)
        {
            _coletorErrors.AddError("MedicalService not found");
            return new Response<MedicalServiceResponse?>(null, 404, "MedicalService not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.MedicalServiceRepository.DisableAsync(MedicalService);
            return new Response<MedicalServiceResponse?>(null, 200, "MedicalService Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting MedicalService");
            return new Response<MedicalServiceResponse?>(null, 404, "Error Deleting MedicalService", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<MedicalServiceResponse>?>> GetAllAsync()
    {
        var MedicalServices = await _uow.MedicalServiceRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<MedicalServiceResponse>>(MedicalServices);
        var count = result.Count();

        return new PagedResponse<IEnumerable<MedicalServiceResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<MedicalServiceResponse>?>> GetByFilterAsync(SearchMedicalServiceRequest request)
    {
        var MedicalServices = await _uow.MedicalServiceRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<MedicalServiceResponse>>(MedicalServices);
        var count = result.Count();

        return new PagedResponse<IEnumerable<MedicalServiceResponse>?>(result, count);
    }

    public async Task<Response<MedicalServiceResponse?>> GetById(long id)
    {
        var MedicalService = await _uow.MedicalServiceRepository.GetByIdAsync(id);

        if (MedicalService == null)
        {
            _coletorErrors.AddError("MedicalService not found");
            return new Response<MedicalServiceResponse?>(null, 404, "MedicalService not found", _coletorErrors.GenerateErrors());
        }

        return new Response<MedicalServiceResponse?>(_mapper.Map<MedicalServiceResponse>(MedicalService), 200, "MedicalService Found", null);
    }

    public async Task<Response<MedicalServiceResponse?>?> Update(MedicalServiceRequest request)
    {
        if (!await Validate(request))
            return new Response<MedicalServiceResponse?>(null, 404, "Error Updating MedicalService", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.MedicalServiceRepository.UpdateAsync(_mapper.Map<MedicalServiceModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating MedicalService");
            return new Response<MedicalServiceResponse?>(null, 404, "Error Updating MedicalService", _coletorErrors.GenerateErrors());
        }

        return new Response<MedicalServiceResponse?>(null, 200, "MedicalService Updated", null);
    }

    private async Task<bool> Validate(MedicalServiceRequest request)
    {
        var Validate = await new MedicalServiceValidator().ValidateAsync(request);

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
