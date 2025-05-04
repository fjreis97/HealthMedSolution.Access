using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.DAL;
using Health_Med.Infrastructure.Repositories.Base;
using Health_Med.Infrastructure.Repositories.Interface;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Infrastructure.Repositories.DoctorByEspecialty;

public class DoctorByEspecialtyRepository(BdHealthMedSession _sessaoBanco) : BaseRepository<DoctorByEspecialtyModel, SearchDoctorByEspecialtyRequest>(_sessaoBanco), IDoctorByEspecialtyRepository
{
    public override string SqlByFilter => $@"SELECT * FROM Registration.tbDoctorByEspecialty WHERE 1 = 1
{sqlWhereFilter}
order by IdDoctor
OFFSET @ResultsToIgnore ROWS
FETCH NEXT @resultsByPage ROWS ONLY; ";

    private const string sqlWhereFilter = @" AND (@IdDoctor IS NULL OR IdDoctor = @IdDoctor)
                                             AND (@IdEspecialty IS NULL OR IdEspecialty = @IdEspecialty)";
}
