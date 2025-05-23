﻿using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Interfaces;

public interface IMedicalScheduleService : IBaseService<Response<MedicalScheduleResponse?>, PagedResponse<IEnumerable<MedicalScheduleResponse>?>, SearchMedicalScheduleRequest, MedicalScheduleRequest>
{
    Task<Response<bool>> UpdateStatusSchedule(MedicalScheduleUpdateStatusRequest request);
    Task<Response<IEnumerable<MedicalScheduleRequest?>?>> GenerateMedicalSchedule(GenerateHoursRequest request);
    Task<Response<bool>> ConfirmedSchedule(long id);
    Task<Response<bool>> RejectedSchedule(long id);
}
