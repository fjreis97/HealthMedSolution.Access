using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Response;

public class InformationDoctorForPatientResponse
{
    public string Crm { get; set; }
    public string? Rqe { get; set; }
    public string Name { get; set; }
}
