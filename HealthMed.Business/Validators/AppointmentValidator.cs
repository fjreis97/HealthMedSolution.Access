using FluentValidation;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class AppointmentValidator : AbstractValidator<AppointmentModel>
{
    public AppointmentValidator()
    {
        RuleFor(x => x.PatientId)
           .NotEmpty().WithMessage("Patient is required");

        RuleFor(x => x.DoctorId)
           .NotEmpty().WithMessage("Doctor is required");

        RuleFor(x => x.SpecialtyId)
           .NotEmpty().WithMessage("Especialty is required");

        RuleFor(x => x.RequestedDate)
           .NotEmpty().WithMessage("Date is required");


        RuleFor(x => x.RequestedTime)
           .NotEmpty().WithMessage("Hora is required");
    }
}
