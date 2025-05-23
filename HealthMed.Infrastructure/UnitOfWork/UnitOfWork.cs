﻿using Health_Med.Infrastructure.DAL;
using Health_Med.Infrastructure.Repositories.Interface;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Health_Med.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{

    private readonly BdHealthMedSession _sessaoBancoDados;
    private bool disposed = false;
    public ICollaboratorRepository CollaboratorRepository { get; }

    public IMedicalEspecialtyRepository MedicalEspecialtyRepository { get; }

    public IMedicalServiceRepository MedicalServiceRepository { get; }

    public IPatientRepository PatientRepository { get; }

    public IDoctorByEspecialtyRepository DoctorByEspecialtyRepository { get; }

    public IDoctorByServiceRepository DoctorByServiceRepository { get; }

    public IDoctorRepository DoctorRepository { get; }
    public IServiceHoursRepository ServiceHoursRepository { get; }
    public IMedicalScheduleRepository MedicalScheduleRepository { get; }
    public IAppointmentRepository AppointmentRepository { get; }


    public UnitOfWork(BdHealthMedSession sessaoBanco,
        ICollaboratorRepository collaboratorRepository,
        IMedicalEspecialtyRepository medicalEspecialtyRepository,
        IMedicalServiceRepository medicalServiceRepository,
        IPatientRepository patientRepository,
        IDoctorByEspecialtyRepository doctorByEspecialtyRepository,
        IDoctorByServiceRepository doctorByServiceRepository,
        IDoctorRepository doctorRepository,
        IServiceHoursRepository serviceHoursRepository,
        IMedicalScheduleRepository medicalScheduleRepository,
        IAppointmentRepository appointmentRepository)
    {
        _sessaoBancoDados = sessaoBanco;
        CollaboratorRepository = collaboratorRepository;
        MedicalEspecialtyRepository = medicalEspecialtyRepository;
        MedicalServiceRepository = medicalServiceRepository;
        PatientRepository = patientRepository;
        DoctorByEspecialtyRepository = doctorByEspecialtyRepository;
        DoctorByServiceRepository = doctorByServiceRepository;
        DoctorRepository = doctorRepository;
        ServiceHoursRepository = serviceHoursRepository;
        MedicalScheduleRepository = medicalScheduleRepository;
        AppointmentRepository = appointmentRepository;
    }

    public void Begin()
    {
        if (_sessaoBancoDados.Connection.State == ConnectionState.Closed)
        {
            _sessaoBancoDados.Connection.Open();
        }
        _sessaoBancoDados.Transaction = _sessaoBancoDados.Connection.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            _sessaoBancoDados.Transaction.Commit();
            _sessaoBancoDados.Transaction.Dispose();

        }
        catch
        {
            _sessaoBancoDados.Transaction.Rollback();
            _sessaoBancoDados.Transaction.Dispose();
        }

    }

    public void Rollback()
    {
        _sessaoBancoDados.Transaction.Rollback();
        _sessaoBancoDados.Transaction.Dispose();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Liberar recursos gerenciados
                _sessaoBancoDados.Connection.Dispose();
                _sessaoBancoDados.Transaction?.Dispose();
            }

            // Liberar recursos não gerenciados, se houver

            disposed = true;
        }
    }
}
