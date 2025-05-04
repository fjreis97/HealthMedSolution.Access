using AutoMapper;
using Health_Med.Business;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Acess.Test;

public class AppointmentServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IColetorErrors> _mockColetorErrors;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockColetorErrors = new Mock<IColetorErrors>();
        _mockConfig = new Mock<IConfiguration>();

        _service = new AppointmentService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockColetorErrors.Object,
            _mockConfig.Object
        );
    }

    //TODO AJUSTAR
    //[Fact]
    //public async Task Create_ShouldCreateAppointment_WhenValidRequest()
    //{
    //    Arrange
    //   var request = new AppointmentRequest { PatientId = 4, DoctorId = 1, SpecialtyId = 1, RequestedDate = new DateTime(2025, 05, 09), RequestedTime = new TimeSpan(0, 16, 0, 0, 319), Status = 1, PatientNote = "agendeei", RequestedAt = DateTime.Now };
    //    var medicalSchedule = new MedicalScheduleResponse { DoctorId = 1, SpecialtyId = 1, Date = DateTime.Now, StartTime = new TimeSpan(0, 16, 00, 00, 319), EndTime = new TimeSpan(0, 16, 50, 00, 319), Status = "Busy", AppointmentId = 1, MotiveCancellation = null, Id = 10 };
    //    var appointmentModel = new AppointmentModel { PatientId = 4, DoctorId = 1, SpecialtyId = 1, RequestedDate = new DateTime(2025, 05, 30), RequestedTime = new TimeSpan(0, 16, 00, 00, 319), Status = 2, PatientNote = "agendeei", RequestedAt = DateTime.Now };

    //    _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.GetByFilterAsync(It.IsAny<SearchMedicalScheduleRequest>()))
    //        .ReturnsAsync(new List<MedicalScheduleResponse> { medicalSchedule });
    //    _mockMapper.Setup(m => m.Map<AppointmentModel>(request)).Returns(appointmentModel);
    //    _mockUnitOfWork.Setup(u => u.AppointmentRepository.InsertAsync(appointmentModel)).ReturnsAsync(1);
    //    _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.UpdateAsync(It.IsAny<MedicalScheduleRequest>())).ReturnsAsync(true);

    //    Act
    //   var result = await _service.Create(request);

    //    Assert
    //    Assert.NotNull(result);
    //    Assert.True(result.IsSuccess);
    //    Assert.Equal("Appointment Created", result.Message);
    //}

    [Fact]
    public async Task Create_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var request = new AppointmentRequest { /* Propriedades inválidas */ };

        _mockColetorErrors.Setup(c => c.GenerateErrors()).Returns(new List<string> { "Validation Error" });

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Creating Appointment", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldDeleteAppointment_WhenValidId()
    {
        // Arrange
        var appointment = new AppointmentModel { Id = 1 };

        _mockUnitOfWork.Setup(u => u.AppointmentRepository.GetByIdAsync(1)).ReturnsAsync(appointment);
        _mockUnitOfWork.Setup(u => u.AppointmentRepository.DisableAsync(appointment)).ReturnsAsync(true);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Appointment Deleted", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnError_WhenAppointmentNotFound()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.AppointmentRepository.GetByIdAsync(1)).ReturnsAsync((AppointmentModel?)null);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Appointment not found", result.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAppointments()
    {
        // Arrange
        var appointments = new List<AppointmentModel>
        {
            new AppointmentModel { Id = 1 },
            new AppointmentModel { Id = 2 }
        };
        var responses = new List<AppointmentResponse>
        {
            new AppointmentResponse { Id = 1 },
            new AppointmentResponse { Id = 2 }
        };

        _mockUnitOfWork.Setup(u => u.AppointmentRepository.GetAllAsync()).ReturnsAsync(appointments);
        _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentResponse>>(appointments)).Returns(responses);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Data?.Count());
    }

    [Fact]
    public async Task GetById_ShouldReturnAppointment_WhenValidId()
    {
        // Arrange
        var appointment = new AppointmentModel { Id = 1 };
        var response = new AppointmentResponse { Id = 1 };

        _mockUnitOfWork.Setup(u => u.AppointmentRepository.GetByIdAsync(1)).ReturnsAsync(appointment);
        _mockMapper.Setup(m => m.Map<AppointmentResponse>(appointment)).Returns(response);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Appointment Found", result.Message);
    }

    //TODO: AJUSTAR
    //[Fact]
    //public async Task Update_ShouldUpdateAppointment_WhenValidRequest()
    //{
    //    // Arrange
    //    var request = new AppointmentRequest { /* Propriedades */ };
    //    var model = new AppointmentModel { /* Propriedades */ };

    //    _mockMapper.Setup(m => m.Map<AppointmentModel>(request)).Returns(model);
    //    _mockUnitOfWork.Setup(u => u.AppointmentRepository.UpdateAsync(model)).ReturnsAsync(true);

    //    // Act
    //    var result = await _service.Update(request);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.True(result.IsSuccess);
    //    Assert.Equal("Appointment Updated", result.Message);
    //}
}
