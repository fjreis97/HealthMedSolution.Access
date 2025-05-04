using FluentValidation;
using Health_Med.Domain.Enums;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class MedicalServiceValidator : AbstractValidator<MedicalServiceModel>
{
    public MedicalServiceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(150).WithMessage("Name can't be longer than 150 characters");

        RuleFor(x => x.IdMedicalEspecialty)
            .NotEmpty().WithMessage("Especialty is required");
    }
}
