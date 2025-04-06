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

namespace Health_Med.Infrastructure.Repositories.Patient;

public class PatientRepository(BdHealthMedSession _sessaoBanco) : BaseRepository<PatientModel, SearchPatientRequest>(_sessaoBanco), IPatientRepository
{
    public override string SqlByFilter => "SELECT * FROM Registration.tbPatient WHERE 1 = 1";
}