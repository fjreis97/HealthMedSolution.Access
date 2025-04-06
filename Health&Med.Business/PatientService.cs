using AutoMapper;
using Health_Med.Business.Interfaces;
using Health_Med.Business.Validators;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using HealthMed.API.Access.Common.RequestDefault;
using HealthMed.API.Access.Common.ResponseDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Health_Med.Business;

public class PatientService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors, IPasswordGenerate _passwordGenerate, IPasswordGenerate _hash) : IPatientService
{
    public async Task<Response<PatientResponse?>?> Create(PatientRequest request)
    {
        if (!await ValidatePatient(request))
            return new Response<PatientResponse?>(null, 404, "Error Creating Patient", _coletorErrors.GenerateErrors());

        try
        {
            request.Password = _hash.GeneratePasswordHash(request.Password);
            long newCode = await _uow.PatientRepository.InsertAsync(_mapper.Map<PatientModel>(request));

            if (newCode == 0)
            {
                _coletorErrors.AddError("Error Creating Patient");
                return new Response<PatientResponse?>(null, 404, "Error Creating Patient", _coletorErrors.GenerateErrors());
            }

            return new Response<PatientResponse?>(null, 200, "Patient Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating Patient");
            return new Response<PatientResponse?>(null, 404, "Error Creating Patient", _coletorErrors.GenerateErrors());
        }

    }

    public async Task<Response<PatientResponse?>?> Delete(long id)
    {
        var patient = await _uow.PatientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            _coletorErrors.AddError("Patient not found");
            return new Response<PatientResponse?>(null, 404, "Patient not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.PatientRepository.DisableAsync(patient);
            return new Response<PatientResponse?>(null, 200, "Patient Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting Patient");
            return new Response<PatientResponse?>(null, 404, "Error Deleting Patient", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<PatientResponse>?>> GetAllAsync()
    {

        var patients = await _uow.PatientRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<PatientResponse>>(patients);
        var count = result.Count();

        return new PagedResponse<IEnumerable<PatientResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<PatientResponse>?>> GetByFilterAsync(SearchPatientRequest request)
    {
        var patients = await _uow.PatientRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<PatientResponse>>(patients);
        var count = result.Count();

        return new PagedResponse<IEnumerable<PatientResponse>?>(result, count);

    }

    public async Task<Response<PatientResponse?>> GetById(long id)
    {
        var patient = await _uow.PatientRepository.GetByIdAsync(id);

        if (patient == null)
        {
            _coletorErrors.AddError("Patient not found");
            return new Response<PatientResponse?>(null, 404, "Patient not found", _coletorErrors.GenerateErrors());
        }

        return new Response<PatientResponse?>(_mapper.Map<PatientResponse>(patient), 200, "Patient Found", null);
    }

    public async Task<Response<PatientResponse?>?> Update(PatientRequest request)
    {

        if (!await ValidatePatient(request))
            return new Response<PatientResponse?>(null, 404, "Error Updating Patient", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.PatientRepository.UpdateAsync(_mapper.Map<PatientModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating Patient");
            return new Response<PatientResponse?>(null, 404, "Error Updating Patient", _coletorErrors.GenerateErrors());
        }

        return new Response<PatientResponse?>(null, 200, "Patient Updated", null);
    }

    public async Task<PatientResponse?> VerifyExistence(LoginRequest requestInitial)
    {
        var collaborator = (await GetAllAsync()).Data?.Where(x => x.EmailAddress.ToLower() == requestInitial.Input.ToLower()).First();

        if (collaborator == null)
            return null; 

        collaborator = _passwordGenerate.ValidadePassword(requestInitial.Password, collaborator.Password) ? collaborator : null;
        return _mapper.Map<PatientResponse>(collaborator);
    }

    private async Task<bool> ValidatePatient(PatientRequest request)
    {
        var validatorPatient = await new PatientValidator().ValidateAsync(request);

        if (!validatorPatient.IsValid)
        {

            foreach (var error in validatorPatient.Errors)
            {
                _coletorErrors.AddError(error.ErrorMessage);
            }
            return false;
        }
        return true;
    }

    public async Task<Response<PatientResponse?>?> DeleteSensitiveData(long id)
    {
        var searchData = await _uow.PatientRepository.GetByIdAsync(id);

        if(searchData is null)
        {
            _coletorErrors.AddError("Patient not found");
            return new Response<PatientResponse?>(null, 404, "Error deleting data", _coletorErrors.GenerateErrors());
        }

        var guid = Guid.NewGuid();

        searchData.Cpf = string.Empty;
        searchData.DateOfBirth = DateTime.Now;
        searchData.Rg = string.Empty;
        searchData.EmailAddress = $"lgpd{guid}@Health.com";
        searchData.Name = $"lgpd{guid}";
        searchData.Active = false;

        var isUpdate = await _uow.PatientRepository.UpdateAsync(searchData);

        if(!isUpdate)
            return new Response<PatientResponse?>(null, 404, "Error deleting data", _coletorErrors.GenerateErrors());


        return new Response<PatientResponse?>(null, 200, "data successfully deleted", _coletorErrors.GenerateErrors());

    }
}
