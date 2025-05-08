using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Health_Med.Domain.Model;

//horarios de atendimento
[Table("Registration.ServiceHours")]
public class ServiceHoursModel
{

    public int Id { get; set; }
    public long DoctorId { get; set; }
    public long EspecialtyId { get; set; }
    public int DayWeek { get; set; }

    [JsonIgnore]
    public TimeSpan HourInit { get; set; }
    [JsonIgnore]
    public TimeSpan HourEnd { get; set; }
    

    [Write(false)]
    public DateTime HourInitNormalize { get; set; }
    [Write(false)]
    public DateTime HourEndNormalize { get; set; }

    // Chamada após a desserialização
    public void NormalizeHours()
    {
        HourInit =  HourInitNormalize.TimeOfDay;
        HourEnd = HourEndNormalize.TimeOfDay;
    }
}
