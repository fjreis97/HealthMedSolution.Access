using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Model;
using HealthMed.API.Access.Common.ResponseDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Interfaces;

public interface IDoctorByEspecialtyService :IBaseService<Response<DoctorByEspecialtyResponse?>,PagedResponse<IEnumerable<DoctorByEspecialtyResponse>?>, SearchDoctorByEspecialtyRequest,DoctorByEspecialtyRequest>
{
}
