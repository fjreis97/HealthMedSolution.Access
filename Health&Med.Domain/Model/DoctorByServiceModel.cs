using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Model;

[Table("Registration.tbDoctorByService")]
public class DoctorByServiceModel
{
    public long IdDoctor { get; set; }
    public long IdService { get; set; }
}
