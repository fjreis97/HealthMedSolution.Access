using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.API.Access.Common.RequestDefault;

public class PagedRequest
{
    public int resultsByPage { get; set; } = 100;
    public int ResultsToIgnore { get; set; } = 0;
}
