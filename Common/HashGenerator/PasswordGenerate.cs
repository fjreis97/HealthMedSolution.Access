using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.API.Access.Common.HashGenerator;

public class PasswordGenerate : IPasswordGenerate
{
    public string GeneratePasswordHash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool ValidadePassword(string passwordEntered, string storedhash) => BCrypt.Net.BCrypt.Verify(passwordEntered, storedhash);
}
