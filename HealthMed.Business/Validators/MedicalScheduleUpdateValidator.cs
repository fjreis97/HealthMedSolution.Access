using FluentValidation;
using FluentValidation.Validators;
using Health_Med.Domain.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class MedicalScheduleUpdateValidator :AbstractValidator<MedicalScheduleUpdateStatusRequest>
{
    public MedicalScheduleUpdateValidator()
    {
        RuleFor(x => x.AppointmentId)
            .NotEmpty().WithMessage("Appointment is required");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required");

        RuleFor(x => x.IdMedicalSchedule)
            .NotEmpty().WithMessage("IdMedicalSchedule is required");
    }
}
