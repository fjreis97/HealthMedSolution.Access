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

namespace Health_Med.Infrastructure.Repositories.Appointment;

public class AppointmentRepository(BdHealthMedSession _sessaoBanco) : BaseRepository<AppointmentModel, SearchAppointmentRequest>(_sessaoBanco), IAppointmentRepository
{
    public override string SqlByFilter => $@"SELECT * FROM Interaction.tbAppointment WHERE 1 = 1
{sqlWhereFilter}
ORDER BY Id
OFFSET @ResultsToIgnore ROWS
FETCH NEXT @resultsByPage ROWS ONLY; ";

    private const string sqlWhereFilter = @" AND (@PatientId IS NULL OR PatientId = @PatientId)
                                             AND (@Status IS NULL OR Status = @Status)";

}
