using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Model;

[Table("Registry.tbDoctorByEspecialtyModel")]
public class DoctorByEspecialtyModel
{
    public long IdDoctor { get; set; }
    public long IdEspecialty { get; set; }
    public decimal Price { get; set; }
}
