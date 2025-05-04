using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Request;

public class GenerateHoursRequest
{
    public int daysAhead { get; set; } = 30;
}
