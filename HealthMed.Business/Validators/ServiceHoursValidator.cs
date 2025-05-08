using FluentValidation;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class ServiceHoursValidator: AbstractValidator<ServiceHoursModel>
{
    public ServiceHoursValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("Doctor is required");

        RuleFor(x => x.EspecialtyId)
            .NotEmpty().WithMessage("Especialty is required");

        RuleFor(x => x.HourEndNormalize)
            .NotEmpty().WithMessage("Hour End required");

        RuleFor(x => x.HourInitNormalize)
            .NotEmpty().WithMessage("Hour init required");

        RuleFor(x => x.DayWeek)
            .NotEmpty().WithMessage("Day of Week required");
    }
}
