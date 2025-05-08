using HealthMed.API.Access.Common.RequestDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Request;

public class SearchMedicalScheduleRequest : PagedRequest
{
    public long? DoctorId { get; set; }
    public long? SpecialtyId { get; set; }
    public DateTime? Date { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public string? Status { get; set; }
    public int? AppointmentId { get; set; }
}
