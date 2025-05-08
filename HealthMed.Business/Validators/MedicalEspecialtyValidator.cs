using FluentValidation;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class MedicalEspecialtyValidator :AbstractValidator<MedicalEspecialtyModel>
{
    public MedicalEspecialtyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("name is required");
    }
}
