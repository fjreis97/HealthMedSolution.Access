using FluentValidation;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class MedicalScheduleValidator :AbstractValidator<MedicalScheduleModel>
{
    public MedicalScheduleValidator()
    {
        RuleFor(x => x.DoctorId)
            .NotEmpty().WithMessage("Doctor is required");

        RuleFor(x => x.SpecialtyId)
            .NotEmpty().WithMessage("Specialty is required");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required");

        RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Start time is required");


        RuleFor(x => x.EndTime)
            .NotEmpty().WithMessage("End time is required");

    }
}

