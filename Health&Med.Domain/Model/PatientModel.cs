using Dapper.Contrib.Extensions;
using Health_Med.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Model;

[Table("Registration.tbPatient")]
public class PatientModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Rg { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public DateTime DateOfRegistration { get; set; } = DateTime.Now;
    public string Password { get; set; } = string.Empty;
    [Write(false)]
    public string ConfirmPassword { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public int RoleId { get; set; } = (int)Role.Patient;
}
