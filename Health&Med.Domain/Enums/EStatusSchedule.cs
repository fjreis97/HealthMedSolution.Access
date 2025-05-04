using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Enums;

public enum EStatusSchedule
{
    Available = 1,
    Booked = 2,
    Scheduled = 3,
    Canceled = 4,
    Completed = 5
}
