using FluentValidation;
using Health_Med.Business.Utils;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class DoctorValidator  :AbstractValidator<DoctorModel>
{
    public DoctorValidator()
    {
        RuleFor(x => x.Crm)
            .Must(RegistroUtils.IsValidCRM).WithMessage("CRM is invalid");

        RuleFor(x => x.Rqe)
            .Must(RegistroUtils.IsValidRQE).WithMessage("RQE is invalid");
    }
}
