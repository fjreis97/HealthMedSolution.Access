using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.API.Access.Common.ColetorErrors;

public class ColetorErrors : IColetorErrors
{
    private List<string> Errors { get; set;}
    public bool hasError => Errors.Any();

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public List<string> GenerateErrors()
    {
        return Errors;
    }
}
