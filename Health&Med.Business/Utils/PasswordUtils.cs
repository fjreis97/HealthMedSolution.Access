using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Health_Med.Business.Utils;

public static class PasswordUtils
{

    public static bool ContainUppercase(string password)
    {
        return password.Any(char.IsUpper);
    }

    public static bool ContainLowercase(string password)
    {
        return password.Any(char.IsLower);
    }

    public static bool ContainDigit(string password)
    {
        return password.Any(char.IsDigit);
    }

    public static bool ContainSpecialCharacter(string password)
    {
        return Regex.IsMatch(password, @"[\W_]");
    }
}
