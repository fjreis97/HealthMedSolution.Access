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
    public override string SqlByFilter => $@"SELECT * FROM Registration.MedicalSchedule WHERE 1 = 1
{sqlWhereFilter}
ORDER BY Date, StartTime
OFFSET @ResultsToIgnore ROWS
FETCH NEXT @resultsByPage ROWS ONLY; ";

    private const string sqlWhereFilter = @" AND (@DoctorId IS NULL OR DoctorId = @DoctorId)
                                             AND (@SpecialtyId IS NULL OR SpecialtyId = @SpecialtyId)
                                             AND (@Date IS NULL OR Date = @Date) 
                                             AND (@StartTime IS NULL OR StartTime = @StartTime) 
                                             AND (@EndTime IS NULL OR EndTime = @EndTime) 
                                             AND (@Status IS NULL OR Status = @Status)
                                             AND (@AppointmentId IS NULL OR AppointmentId = @AppointmentId)";

}
