using Health_Med.Infrastructure.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Infrastructure.UnitOfWork.Interface;

public interface IUnitOfWork :IDisposable
{
    ICollaboratorRepository CollaboratorRepository { get; }
    IMedicalEspecialtyRepository MedicalEspecialtyRepository { get; }
    IMedicalServiceRepository MedicalServiceRepository { get; }
    IPatientRepository PatientRepository { get; }
    IDoctorByEspecialtyRepository DoctorByEspecialtyRepository { get; }
    IDoctorByServiceRepository DoctorByServiceRepository { get; }
    IDoctorRepository DoctorRepository { get; }


    void Begin();
    void Commit();
    void Rollback();
}
