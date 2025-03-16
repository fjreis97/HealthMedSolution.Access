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

public class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<DoctorModel, DoctorRequest>();
        CreateMap<DoctorModel, DoctorResponse>();
        CreateMap<DoctorRequest, DoctorResponse>();
    }
}
