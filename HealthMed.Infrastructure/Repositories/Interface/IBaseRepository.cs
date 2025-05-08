using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Infrastructure.Repositories.Interface;

public interface IBaseRepository<TModel, TSearchRequest> where TModel : class where TSearchRequest : class
{
    public abstract string SqlByFilter { get; }

    Task<long> InsertAsync(TModel model);
    Task<bool> UpdateAsync(TModel model);
    Task<bool> DisableAsync(TModel model);
    Task<TModel> GetByIdAsync(long id);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task<IEnumerable<TModel>> GetByFilterAsync(TSearchRequest request);

}
