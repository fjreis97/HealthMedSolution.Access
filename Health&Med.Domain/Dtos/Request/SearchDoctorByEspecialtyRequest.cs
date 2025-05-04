using HealthMed.API.Access.Common.RequestDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Request;

public class SearchDoctorByEspecialtyRequest :PagedRequest
{
    public long? IdDoctor { get; set; }
    public long? IdEspecialty { get; set; }
}
