using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Business.Utils;

public static class RGUtils
{
    public static bool IsValidRG(string rg)
    {
        // Remova caracteres não numéricos
        rg = new string(rg.Where(char.IsDigit).ToArray());

        // Verifique se o RG tem entre 7 e 9 dígitos (dependendo do estado)
        if (rg.Length < 7 || rg.Length > 9)
            return false;

        // Você pode adicionar outras regras específicas por estado aqui, se necessário

        return true;
    }
}
