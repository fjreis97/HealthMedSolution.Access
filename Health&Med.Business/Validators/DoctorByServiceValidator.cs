using FluentValidation;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class DoctorByServiceValidator :AbstractValidator<DoctorByServiceModel>
{
    public DoctorByServiceValidator()
    {
        RuleFor(x => x.IdService)
            .NotEmpty().WithMessage("ServiceId is required");

        RuleFor(x => x.IdDoctor)
            .NotEmpty().WithMessage("DoctorId is required");

    }
}

