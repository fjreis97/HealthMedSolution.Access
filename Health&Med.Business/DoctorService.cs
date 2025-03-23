using AutoMapper;
using Health_Med.Business.Interfaces;
using Health_Med.Business.Validators;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using HealthMed.API.Access.Common.ResponseDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business;

public class DoctorService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors) : IDoctorService
{
    public async Task<Response<DoctorResponse?>?> Create(DoctorRequest request)
    {
        if (!await ValidateDoctor(request))
            return new Response<DoctorResponse?>(null, 404, "Error Creating Doctor", _coletorErrors.GenerateErrors());

        try
        {
            long newCode = await _uow.DoctorRepository.InsertAsync(_mapper.Map<DoctorModel>(request));

            if (newCode == 0)
            {
                _coletorErrors.AddError("Error Creating Doctor");
                return new Response<DoctorResponse?>(null, 404, "Error Creating Doctor", _coletorErrors.GenerateErrors());
            }

            return new Response<DoctorResponse?>(null, 200, "Doctor Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating Doctor");
            return new Response<DoctorResponse?>(null, 404, "Error Creating Doctor", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<DoctorResponse?>?> Delete(long id)
    {
        var Doctor = await _uow.DoctorRepository.GetByIdAsync(id);

        if (Doctor == null)
        {
            _coletorErrors.AddError("Doctor not found");
            return new Response<DoctorResponse?>(null, 404, "Doctor not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.DoctorRepository.DisableAsync(Doctor);
            return new Response<DoctorResponse?>(null, 200, "Doctor Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting Doctor");
            return new Response<DoctorResponse?>(null, 404, "Error Deleting Doctor", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<DoctorResponse>?>> GetAllAsync()
    {
        var Doctors = await _uow.DoctorRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<DoctorResponse>>(Doctors);
        var count = result.Count();

        return new PagedResponse<IEnumerable<DoctorResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<DoctorResponse>?>> GetByFilterAsync(SearchDoctorRequest request)
    {
        var Doctors = await _uow.DoctorRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<DoctorResponse>>(Doctors);
        var count = result.Count();

        return new PagedResponse<IEnumerable<DoctorResponse>?>(result, count);
    }

    public async Task<Response<DoctorResponse?>> GetById(long id)
    {
        var Doctor = await _uow.DoctorRepository.GetByIdAsync(id);

        if (Doctor == null)
        {
            _coletorErrors.AddError("Doctor not found");
            return new Response<DoctorResponse?>(null, 404, "Doctor not found", _coletorErrors.GenerateErrors());
        }

        return new Response<DoctorResponse?>(_mapper.Map<DoctorResponse>(Doctor), 200, "Doctor Found", null);
    }

    public async Task<Response<DoctorResponse?>> Update(DoctorRequest request)
    {
        if (!await ValidateDoctor(request))
            return new Response<DoctorResponse?>(null, 404, "Error Updating Doctor", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.DoctorRepository.UpdateAsync(_mapper.Map<DoctorModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating Doctor");
            return new Response<DoctorResponse?>(null, 404, "Error Updating Doctor", _coletorErrors.GenerateErrors());
        }

        return new Response<DoctorResponse?>(null, 200, "Doctor Updated", null);
    }

    private async Task<bool> ValidateDoctor(DoctorRequest request)
    {
        var validator = await new DoctorValidator().ValidateAsync(request);

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
