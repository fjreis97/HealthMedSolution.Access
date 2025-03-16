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
        RuleFor(x => x.Name)
       .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("Cpf is required")
            .Must(CPFUtils.IsValidCPF).WithMessage("Cpf is invalid");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("BirthDate is required")
            .Must(birthDate => birthDate < DateTime.Now).WithMessage("BirthDate is invalid");

        RuleFor(x => x.Rg)
            .Must(RGUtils.IsValidRG).WithMessage("Rg is invalid");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .EmailAddress().WithMessage("Address is invalid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters")
            .Must(PasswordUtils.ContainUppercase).EmailAddress().WithMessage("Password must have at least 1 uppercase character")
            .Must(PasswordUtils.ContainLowercase).MinimumLength(6).WithMessage("Password must have at least 1 lowercase character")
            .Must(PasswordUtils.ContainDigit).MaximumLength(20).WithMessage("Password must have at least 1 digit")
            .Must(PasswordUtils.ContainSpecialCharacter).MaximumLength(20).WithMessage("Password must have at least 1 special character");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("RoleId is required");


        RuleFor(x => x.Crm)
            .Must(RegistroUtils.IsValidCRM).WithMessage("CRM is invalid");

        RuleFor(x => x.Rqe)
            .Must(RegistroUtils.IsValidRQE).WithMessage("RQE is invalid");
    }
}
