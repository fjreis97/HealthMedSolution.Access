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

namespace Health_Med.Infrastructure.Repositories.DoctorByService;

public class DoctorByServiceRepository(BdHealthMedSession _sessaoBanco) : BaseRepository<DoctorByServiceModel, SearchDoctorByServiceRequest>(_sessaoBanco), IDoctorByServiceRepository
{
    public override string SqlByFilter => $@"SELECT * FROM Registration.tbDoctorByService WHERE 1 = 1
ORDER BY Id
OFFSET @ResultsToIgnore ROWS
FETCH NEXT @resultsByPage ROWS ONLY; ";
}

