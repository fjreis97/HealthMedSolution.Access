using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.API.Access.Common.ColetorErrors.Interfaces;

public interface IColetorErrors
{
    public bool hasError { get;}
    void AddError(string error);
    List<string> GenerateErrors();
}
