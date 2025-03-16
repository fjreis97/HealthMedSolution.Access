using HealthMed.API.Access.Common.ResponseDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Interfaces;

public interface IBaseService<TResponse, TPagedResponse, TSearchRequest, TRequest>
{
    Task<TResponse?> Create(TRequest request);
    Task<TResponse?> Update(TRequest request);
    Task<TResponse?> Delete(long id);
    Task<TResponse> GetById(long id);
    Task<TPagedResponse> GetAllAsync();
    Task<TPagedResponse> GetByFilterAsync(TSearchRequest request);

}
