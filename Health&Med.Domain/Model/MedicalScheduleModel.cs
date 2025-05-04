using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Model;

[Table("Registration.MedicalSchedule")]
public class MedicalScheduleModel
{
    public int Id { get; set; }
    public long DoctorId { get; set; }
    public long SpecialtyId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; } = "Available";
    public int? AppointmentId { get; set; }
    public string? MotiveCancellation { get; set; }
}
