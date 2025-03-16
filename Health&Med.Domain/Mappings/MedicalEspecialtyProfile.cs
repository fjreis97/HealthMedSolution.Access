using AutoMapper;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Domain.Mappings;

public class MedicalEspecialtyProfile :Profile
{
    public MedicalEspecialtyProfile()
    {
        CreateMap<MedicalEspecialtyModel, MedicalEspecialtyRequest>();
        CreateMap<MedicalEspecialtyModel, MedicalEspecialtyResponse>();
        CreateMap<MedicalEspecialtyRequest, MedicalEspecialtyResponse>();
    }
}
