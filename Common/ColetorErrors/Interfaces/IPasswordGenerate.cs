using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.API.Access.Common.ColetorErrors.Interfaces;

public interface IPasswordGenerate
{
    public string GeneratePasswordHash(string password);
    public bool ValidadePassword(string passwordEntered, string storedhash);
}
