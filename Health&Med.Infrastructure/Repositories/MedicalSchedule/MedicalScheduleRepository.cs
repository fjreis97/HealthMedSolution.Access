using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.DAL;
using Health_Med.Infrastructure.Repositories.Base;
using Health_Med.Infrastructure.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Infrastructure.Repositories.MedicalSchedule;

public class MedicalScheduleRepository(BdHealthMedSession _sessaoBanco) : BaseRepository<MedicalScheduleModel, SearchMedicalScheduleRequest>(_sessaoBanco), IMedicalScheduleRepository
{
    public override string SqlByFilter => "SELECT * FROM Registration.MedicalSchedule WHERE 1 = 1";

}
