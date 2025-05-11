using Health_Med.Domain.Enums;
using HealthMed.API.Access.Common.RequestDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Request;

public class SearchAppointmentRequest : PagedRequest
{
    public long PatientId { get; set; }
    public int Status { get; set; } 
}
