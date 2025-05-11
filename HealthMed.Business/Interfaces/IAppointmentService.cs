using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Interfaces;

public interface IAppointmentService : IBaseService<Response<AppointmentResponse?>, PagedResponse<IEnumerable<AppointmentResponse>?>, SearchAppointmentRequest, AppointmentRequest>
{
    Task<bool> ApprovedAppointment(long id);
    Task<bool> RejectedAppointment(long id);
    Task<Response<AppointmentResponse?>?> CancelAppointment(long id);
}
