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

public class CollaboratorService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors _coletorErrors, IDoctorService _doctorService, IPasswordGenerate _hash) : ICollaboratorService
{
    public async Task<Response<CollaboratorResponse?>?> Create(CollaboratorRequest request)
    {

        if (!await ValidateCollaborator(request))
            return new Response<CollaboratorResponse?>(null, 404, "Error Creating Collaborator", _coletorErrors.GenerateErrors());

        try
        {
            request.Password = _hash.GeneratePasswordHash(request.Password);
            long newCode = await _uow.CollaboratorRepository.InsertAsync(_mapper.Map<CollaboratorModel>(request));

            if (newCode == 0)
            {
                _coletorErrors.AddError("Error Creating Collaborator");
                return new Response<CollaboratorResponse?>(null, 404, "Error Creating Collaborator", _coletorErrors.GenerateErrors());
            }

            return new Response<CollaboratorResponse?>(null, 204, "Collaborator Created", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Creating Collaborator");
            return new Response<CollaboratorResponse?>(null, 404, "Error Creating Collaborator", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<Response<CollaboratorResponse?>?> Delete(long id)
    {
        var Collaborator = await _uow.CollaboratorRepository.GetByIdAsync(id);

        if (Collaborator == null)
        {
            _coletorErrors.AddError("Collaborator not found");
            return new Response<CollaboratorResponse?>(null, 404, "Collaborator not found", _coletorErrors.GenerateErrors());
        }

        try
        {
            bool isDisabled = await _uow.CollaboratorRepository.DisableAsync(Collaborator);
            return new Response<CollaboratorResponse?>(null, 200, "Collaborator Deleted", null);
        }
        catch (Exception ex)
        {
            _coletorErrors.AddError("Error Deleting Collaborator");
            return new Response<CollaboratorResponse?>(null, 404, "Error Deleting Collaborator", _coletorErrors.GenerateErrors());
        }
    }

    public async Task<PagedResponse<IEnumerable<CollaboratorResponse>?>> GetAllAsync()
    {
        var Collaborators = await _uow.CollaboratorRepository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CollaboratorResponse>>(Collaborators);
        var count = result.Count();

        return new PagedResponse<IEnumerable<CollaboratorResponse>?>(result, count);
    }

    public async Task<PagedResponse<IEnumerable<CollaboratorResponse>?>> GetByFilterAsync(SearchCollaboratorRequest request)
    {
        var Collaborators = await _uow.CollaboratorRepository.GetByFilterAsync(request);
        var result = _mapper.Map<IEnumerable<CollaboratorResponse>>(Collaborators);
        var count = result.Count();

        return new PagedResponse<IEnumerable<CollaboratorResponse>?>(result, count);
    }

    public async Task<Response<CollaboratorResponse?>> GetById(long id)
    {
        var Collaborator = await _uow.CollaboratorRepository.GetByIdAsync(id);

        if (Collaborator == null)
        {
            _coletorErrors.AddError("Collaborator not found");
            return new Response<CollaboratorResponse?>(null, 404, "Collaborator not found", _coletorErrors.GenerateErrors());
        }

        return new Response<CollaboratorResponse?>(_mapper.Map<CollaboratorResponse>(Collaborator), 200, "Collaborator Found", null);
    }

    public async Task<Response<CollaboratorResponse?>?> Update(CollaboratorRequest request)
    {
        if (!await ValidateCollaborator(request))
            return new Response<CollaboratorResponse?>(null, 404, "Error Updating Collaborator", _coletorErrors.GenerateErrors());

        var IsUpdate = await _uow.CollaboratorRepository.UpdateAsync(_mapper.Map<CollaboratorModel>(request));

        if (!IsUpdate)
        {
            _coletorErrors.AddError("Error Updating Collaborator");
            return new Response<CollaboratorResponse?>(null, 404, "Error Updating Collaborator", _coletorErrors.GenerateErrors());
        }

        return new Response<CollaboratorResponse?>(null, 200, "Collaborator Updated", null);
    }

    public async Task<CollaboratorResponse?> VerifyExistence(LoginRequest requestInitial)
    {
        bool isEmail = requestInitial.Input.Contains("@"); 
        CollaboratorModel? collaborator = null;
        var listCollaborator = await _uow.CollaboratorRepository.GetAllAsync();

        if (isEmail)
        {        
            collaborator = listCollaborator.FirstOrDefault(c => c.EmailAddress == requestInitial.Input);

            if (collaborator == null)
                return null;

            collaborator = _hash.ValidadePassword(requestInitial.Password, collaborator.Password) ? collaborator : null;
            return _mapper.Map<CollaboratorResponse>(collaborator);
        }
        
        var searchCollaboratorDoctor = (await _doctorService.GetAllAsync()).Data?.Where(x => x.Crm.ToLower() == requestInitial.Input.ToLower());
        collaborator = listCollaborator.Where(x => x.Id == searchCollaboratorDoctor?.First().Id).First();


        if (collaborator == null)
            return null;

        collaborator = _hash.ValidadePassword(requestInitial.Password, collaborator.Password) ? collaborator : null;
        return _mapper.Map<CollaboratorResponse>(collaborator);
    }

    private async Task<bool> ValidateCollaborator(CollaboratorRequest request)
    {
        var validator = await new CollaboratorValidator().ValidateAsync(request);

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
