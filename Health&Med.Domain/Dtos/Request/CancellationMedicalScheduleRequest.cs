using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Dtos.Request;

public class CancellationMedicalScheduleRequest
{
    [Required(ErrorMessage = "Id da consulta não informado")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Motivo de cancelamento não informado")]
    public string MotiveCancellation { get; set; }
}
