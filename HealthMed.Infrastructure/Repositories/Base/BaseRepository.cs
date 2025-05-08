using Dapper.Contrib.Extensions;
using Health_Med.Infrastructure.DAL;
using Health_Med.Infrastructure.Repositories.Interface;
using Health_Med.Infrastructure.UnitOfWork;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Health_Med.Infrastructure.Repositories.Base;

public abstract class BaseRepository<TModel, TSearchRequest>(BdHealthMedSession _sessaoBanco) : IBaseRepository<TModel, TSearchRequest> where TModel : class where TSearchRequest : class
{

    public abstract string SqlByFilter { get; }
    public async Task<IEnumerable<TModel>> GetAllAsync() => await _sessaoBanco.Connection.GetAllAsync<TModel>();
    public async Task<IEnumerable<TModel>> GetByFilterAsync(TSearchRequest request) => await _sessaoBanco.Connection.QueryAsync<TModel>(SqlByFilter, request, _sessaoBanco.Transaction);
    public async Task<TModel> GetByIdAsync(long id) => await _sessaoBanco.Connection.GetAsync<TModel>(id, _sessaoBanco.Transaction);
    public async Task<long> InsertAsync(TModel model) => await _sessaoBanco.Connection.InsertAsync(model, _sessaoBanco.Transaction);
    public async Task<bool> UpdateAsync(TModel model) => await _sessaoBanco.Connection.UpdateAsync(model, _sessaoBanco.Transaction);
    public async Task<bool> DisableAsync(TModel model) => await _sessaoBanco.Connection.UpdateAsync(model, _sessaoBanco.Transaction);  // delete lógico, apenas alterando o active para false

}
