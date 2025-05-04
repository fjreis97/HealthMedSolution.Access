using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Response;

public class DoctorByEspecialtyResponse : DoctorByEspecialtyModel
{
    public InformationDoctorForPatientResponse? doctor { get; set; }
    public string especialty { get; set; }
}
