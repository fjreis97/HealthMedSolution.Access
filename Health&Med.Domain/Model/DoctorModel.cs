using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Model;

[Table("Registry.tbDoctorModel")]
public class DoctorModel : CollaboratorModel
{
    public string Crm { get; set; }
    public string Rqe { get; set; }
}
