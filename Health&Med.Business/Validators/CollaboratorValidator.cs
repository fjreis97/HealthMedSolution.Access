using FluentValidation;
using Health_Med.Business.Utils;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Validators;

public class CollaboratorValidator: AbstractValidator<CollaboratorModel>
{
    public CollaboratorValidator()
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
            .NotEmpty().WithMessage("Rg is required")
            .Must(RGUtils.IsValidRG).WithMessage("Rg is invalid");

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email Address is required")
            .EmailAddress().WithMessage("Email Address is invalid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters")
            .MaximumLength(20).WithMessage("Password must have at most 20 characters")
            .Must(PasswordUtils.ContainUppercase).WithMessage("Password must have at least 1 uppercase character")
            .Must(PasswordUtils.ContainLowercase).WithMessage("Password must have at least 1 lowercase character")
            .Must(PasswordUtils.ContainDigit).WithMessage("Password must have at least 1 digit")
            .Must(PasswordUtils.ContainSpecialCharacter).WithMessage("Password must have at least 1 special character");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required")
            .Equal(x => x.Password).WithMessage("passwords don't match");

        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage("RoleId is required");
    }
}
