using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Request;

public class MedicalScheduleUpdateStatusRequest
{
    public int IdMedicalSchedule { get; set; }
    public string Status { get; set; } = string.Empty;
    public int AppointmentId { get; set; }
}
