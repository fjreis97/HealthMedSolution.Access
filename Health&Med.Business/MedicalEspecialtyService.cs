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

public class MedicalEspecialtyService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors) : IMedicalEspecialtyService
{
    public async Task<Response<MedicalEspecialtyResponse?>> Create(MedicalEspecialtyRequest request)
    {
        if (!await Validate(request))
            return new Response<MedicalEspecialtyResponse?>(null, 404, "Error Creating MedicalEspecialty", _coletorErrors.GenerateErrors());

        try
        {
            long newCode = await _uow.MedicalEspecialtyRepository.InsertAsync(_mapper.Map<MedicalEspecialtyModel>(request));

            if (newCode == 0)
            {
                _coletorErrors.AddError("Error Creating MedicalEspecialty");
                return new Response<MedicalEspecialtyResponse?>(null, 404, "Error Creating MedicalEspecialty", _coletorErrors.GenerateErrors());
            }

            return new Response<MedicalEspecialtyResponse?>(null, 200, "MedicalEspecialty Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating MedicalEspecialty");
            return new Response<MedicalEspecialtyResponse?>(null, 404, "Error Creating MedicalEspecialty", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<MedicalEspecialtyResponse>?> Delete(long id)
    {
        var MedicalEspecialty = await _uow.MedicalEspecialtyRepository.GetByIdAsync(id);

        if (MedicalEspecialty == null)
        {
            _coletorErrors.AddError("MedicalEspecialty not found");
            return new Response<MedicalEspecialtyResponse?>(null, 404, "MedicalEspecialty not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.MedicalEspecialtyRepository.DisableAsync(MedicalEspecialty);
            return new Response<MedicalEspecialtyResponse?>(null, 200, "MedicalEspecialty Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting MedicalEspecialty");
            return new Response<MedicalEspecialtyResponse?>(null, 404, "Error Deleting MedicalEspecialty", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<MedicalEspecialtyResponse>>> GetAllAsync()
    {
        var MedicalEspecialtys = await _uow.MedicalEspecialtyRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<MedicalEspecialtyResponse>>(MedicalEspecialtys);

        foreach(var item in result)
        {
            item.MedicalServices = _mapper.Map<IEnumerable<MedicalServiceResponse>>((await _uow.MedicalServiceRepository.GetAllAsync()).Where(x => x.IdMedicalEspecialty == item.Id).ToList());
        }
        var count = result.Count();

        return new PagedResponse<IEnumerable<MedicalEspecialtyResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<MedicalEspecialtyResponse>>> GetByFilterAsync(SearchMedicalEspecialtyRequest request)
    {
        var MedicalEspecialtys = await _uow.MedicalEspecialtyRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<MedicalEspecialtyResponse>>(MedicalEspecialtys);
        var count = result.Count();

        return new PagedResponse<IEnumerable<MedicalEspecialtyResponse>?>(result, count);
    }

    public async Task<Response<MedicalEspecialtyResponse>> GetById(long id)
    {
        var MedicalEspecialty = await _uow.MedicalEspecialtyRepository.GetByIdAsync(id);

        if (MedicalEspecialty == null)
        {
            _coletorErrors.AddError("MedicalEspecialty not found");
            return new Response<MedicalEspecialtyResponse?>(null, 404, "MedicalEspecialty not found", _coletorErrors.GenerateErrors());
        }

        return new Response<MedicalEspecialtyResponse?>(_mapper.Map<MedicalEspecialtyResponse>(MedicalEspecialty), 200, "MedicalEspecialty Found", null);
    }

    public async Task<Response<MedicalEspecialtyResponse>?> Update(MedicalEspecialtyRequest request)
    {

        if (!await Validate(request))
            return new Response<MedicalEspecialtyResponse?>(null, 404, "Error Updating MedicalEspecialty", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.MedicalEspecialtyRepository.UpdateAsync(_mapper.Map<MedicalEspecialtyModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating MedicalEspecialty");
            return new Response<MedicalEspecialtyResponse?>(null, 404, "Error Updating MedicalEspecialty", _coletorErrors.GenerateErrors());
        }

        return new Response<MedicalEspecialtyResponse?>(null, 200, "MedicalEspecialty Updated", null);
    }

    private async Task<bool> Validate(MedicalEspecialtyRequest request)
    {
        var validator = await new MedicalEspecialtyValidator().ValidateAsync(request);

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
