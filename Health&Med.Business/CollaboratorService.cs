using AutoMapper;
using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business;

public class CollaboratorService(IUnitOfWork _uow, IMapper _mapper, IColetorErrors coletorErrors) : ICollaboratorService
{
    public Task<CollaboratorModel?> Create(CollaboratorRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CollaboratorModel?> Delete(CollaboratorRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CollaboratorModel?>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CollaboratorModel?>> GetByFilterAsync(SearchCollaboratorRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<CollaboratorModel> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<CollaboratorModel?> Update(CollaboratorRequest request)
    {
        throw new NotImplementedException();
    }
}
