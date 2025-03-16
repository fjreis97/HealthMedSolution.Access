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

namespace Health_Med.Infrastructure.Repositories.Doctor;

public class DoctorRepository(BdHealthMedSession _sessaoBanco): BaseRepository<DoctorModel, SearchDoctorRequest>(_sessaoBanco),IDoctorRepository
{
    public override string SqlByFilter => "SELECT * FROM tbDoctor WHERE 1 = 1";
}

