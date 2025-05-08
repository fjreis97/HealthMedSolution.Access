using Dapper.Contrib.Extensions;
using Health_Med.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Model;

[Table("Interaction.tbAppointment")]
public class AppointmentModel
{
    public int Id { get; set; }
    public long PatientId { get; set; }
    public long DoctorId { get; set; }
    public long SpecialtyId { get; set; }
    public DateTime RequestedDate { get; set; }      
    public TimeSpan RequestedTime { get; set; }      
    public int Status { get; set; } = (int)EStatus.Pending;
    public string? PatientNote { get; set; }
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}
