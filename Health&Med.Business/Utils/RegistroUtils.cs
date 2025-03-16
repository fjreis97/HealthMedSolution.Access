using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Health_Med.Business.Utils;

public static class RegistroUtils
{
    public static bool IsValidCRM(string crm)
    {
        // Exemplo: Verifique se o CRM tem um formato básico de 4 a 6 dígitos seguido de uma sigla de 2 a 3 letras (ex: 12345-SP)
        return Regex.IsMatch(crm, @"^\d{4,6}-[A-Z]{2,3}$");
    }

    public static bool IsValidRQE(string rqe)
    {
        // Exemplo: Verifique se o RQE tem apenas dígitos e um comprimento razoável (4 a 6 dígitos)
        return Regex.IsMatch(rqe, @"^\d{4,6}$");
    }
}
