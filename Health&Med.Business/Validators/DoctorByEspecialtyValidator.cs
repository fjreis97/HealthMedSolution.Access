using FluentValidation;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class DoctorByEspecialtyValidator :AbstractValidator<DoctorByEspecialtyModel>
{
    public DoctorByEspecialtyValidator()
    {
        RuleFor(x => x.IdEspecialty)
            .NotEmpty().WithMessage("EspecialtyId is required");

        RuleFor(x => x.IdDoctor)
            .NotEmpty().WithMessage("DoctorId is required");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}
