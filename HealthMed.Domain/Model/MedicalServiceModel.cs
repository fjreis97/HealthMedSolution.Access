﻿using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Model;

[Table("Registration.tbMedicalService")]
public class MedicalServiceModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public long IdMedicalEspecialty { get; set; }
}
