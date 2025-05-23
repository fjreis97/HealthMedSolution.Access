﻿using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Infrastructure.Repositories.Interface;

public interface IMedicalEspecialtyRepository : IBaseRepository<MedicalEspecialtyModel, SearchMedicalEspecialtyRequest>
{
}
