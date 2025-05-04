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

public class DoctorByEspecialtyService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors) : IDoctorByEspecialtyService
{
    public async Task<Response<DoctorByEspecialtyResponse?>?> Create(DoctorByEspecialtyRequest request)
    {
        if (!await ValidateDoctorByEspecialty(request))
            return new Response<DoctorByEspecialtyResponse?>(null, 404, "Error Creating DoctorByEspecialty", _coletorErrors.GenerateErrors());

        try
        {
            long newCode = await _uow.DoctorByEspecialtyRepository.InsertAsync(_mapper.Map<DoctorByEspecialtyModel>(request));
            return new Response<DoctorByEspecialtyResponse?>(null, 200, "DoctorByEspecialty Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating DoctorByEspecialty");
            return new Response<DoctorByEspecialtyResponse?>(null, 404, "Error Creating DoctorByEspecialty", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<DoctorByEspecialtyResponse?>?> Delete(long id)
    {
        var DoctorByEspecialty = await _uow.DoctorByEspecialtyRepository.GetByIdAsync(id);

        if (DoctorByEspecialty == null)
        {
            _coletorErrors.AddError("DoctorByEspecialty not found");
            return new Response<DoctorByEspecialtyResponse?>(null, 404, "DoctorByEspecialty not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.DoctorByEspecialtyRepository.DisableAsync(DoctorByEspecialty);
            return new Response<DoctorByEspecialtyResponse?>(null, 200, "DoctorByEspecialty Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting DoctorByEspecialty");
            return new Response<DoctorByEspecialtyResponse?>(null, 404, "Error Deleting DoctorByEspecialty", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<DoctorByEspecialtyResponse>?>> GetAllAsync()
    {
        var DoctorByEspecialtys = await _uow.DoctorByEspecialtyRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<DoctorByEspecialtyResponse>>(DoctorByEspecialtys);
        var count = result.Count();

        return new PagedResponse<IEnumerable<DoctorByEspecialtyResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<DoctorByEspecialtyResponse>?>> GetByFilterAsync(SearchDoctorByEspecialtyRequest request)
    {
        var DoctorByEspecialtys = await _uow.DoctorByEspecialtyRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<DoctorByEspecialtyResponse>>(DoctorByEspecialtys);
        var ListDoctor = await _uow.DoctorRepository.GetAllAsync();
        var listCollaborator = await _uow.CollaboratorRepository.GetAllAsync();
        var listEspecialty = await _uow.MedicalEspecialtyRepository.GetAllAsync();

        foreach(var item in result)
        {
            var doctor = ListDoctor.FirstOrDefault(x => x.Id == item.IdDoctor)!;
            var collaborator = listCollaborator.FirstOrDefault(x => x.Id == doctor.IdCollaborator);
            item.especialty = (listEspecialty.FirstOrDefault(x => x.Id == item.IdEspecialty)!).Name;
            item.doctor = new InformationDoctorForPatientResponse()
            {
                Crm = doctor.Crm,
                Name = collaborator.Name,
                Rqe = doctor.Rqe
            };
        }

        var count = result.Count();

        return new PagedResponse<IEnumerable<DoctorByEspecialtyResponse>?>(result, count);
    }

    public async Task<Response<DoctorByEspecialtyResponse?>> GetById(long id)
    {
        var DoctorByEspecialty = await _uow.DoctorByEspecialtyRepository.GetByIdAsync(id);

        if (DoctorByEspecialty == null)
        {
            _coletorErrors.AddError("DoctorByEspecialty not found");
            return new Response<DoctorByEspecialtyResponse?>(null, 404, "DoctorByEspecialty not found", _coletorErrors.GenerateErrors());
        }

        return new Response<DoctorByEspecialtyResponse?>(_mapper.Map<DoctorByEspecialtyResponse>(DoctorByEspecialty), 200, "DoctorByEspecialty Found", null);
    }

    public async Task<Response<DoctorByEspecialtyResponse?>?> Update(DoctorByEspecialtyRequest request)
    {
        if (!await ValidateDoctorByEspecialty(request))
            return new Response<DoctorByEspecialtyResponse?>(null, 404, "Error Updating DoctorByEspecialty", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.DoctorByEspecialtyRepository.UpdateAsync(_mapper.Map<DoctorByEspecialtyModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating DoctorByEspecialty");
            return new Response<DoctorByEspecialtyResponse?>(null, 404, "Error Updating DoctorByEspecialty", _coletorErrors.GenerateErrors());
        }

        return new Response<DoctorByEspecialtyResponse?>(null, 200, "DoctorByEspecialty Updated", null);
    }

    private async Task<bool> ValidateDoctorByEspecialty(DoctorByEspecialtyRequest request)
    {
        var validatorDoctorByEspecialty = await new DoctorByEspecialtyValidator().ValidateAsync(request);

        if (!validatorDoctorByEspecialty.IsValid)
        {

            foreach (var error in validatorDoctorByEspecialty.Errors)
            {
                _coletorErrors.AddError(error.ErrorMessage);
            }
            return false;
        }
        return true;
    }
}
