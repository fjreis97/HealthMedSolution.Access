using AutoMapper;
using Health_Med.Business;
using Health_Med.Business.Interfaces;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using Moq;

namespace HealthMed.Acess.Test;

public class MedicalScheduleServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IColetorErrors> _mockColetorErrors;
    private readonly Mock<IAppointmentService> _mockAppointmentService;
    private readonly MedicalScheduleService _service;

    public MedicalScheduleServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockColetorErrors = new Mock<IColetorErrors>();
        _mockAppointmentService = new Mock<IAppointmentService>();

        _service = new MedicalScheduleService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockColetorErrors.Object,
            _mockAppointmentService.Object
        );
    }

    [Fact]
    public async Task ConfirmedSchedule_ShouldConfirmSchedule_WhenValidId()
    {
        // Arrange
        var schedule = new MedicalScheduleModel { Id = 1, Status = "Available", AppointmentId = 1 };
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.GetByIdAsync(1)).ReturnsAsync(schedule);
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.UpdateAsync(schedule)).ReturnsAsync(true);
        _mockAppointmentService.Setup(a => a.ApprovedAppointment(1)).ReturnsAsync(true);

        // Act
        var result = await _service.ConfirmedSchedule(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Confirmed MedicalSchedule", result.Message);
    }

    [Fact]
    public async Task ConfirmedSchedule_ShouldReturnError_WhenUpdateFails()
    {
        // Arrange
        var schedule = new MedicalScheduleModel { Id = 1, Status = "Available", AppointmentId = 1 };
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.GetByIdAsync(1)).ReturnsAsync(schedule);
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.UpdateAsync(schedule)).ReturnsAsync(false);
        _mockAppointmentService.Setup(a => a.ApprovedAppointment(1)).ReturnsAsync(false);

        // Act
        var result = await _service.ConfirmedSchedule(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Confirmed MedicalSchedule", result.Message);
    }

    [Fact]
    public async Task RejectedSchedule_ShouldRejectSchedule_WhenValidId()
    {
        // Arrange
        var schedule = new MedicalScheduleModel { Id = 1, Status = "Busy", AppointmentId = 1 };
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.GetByIdAsync(1)).ReturnsAsync(schedule);
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.UpdateAsync(schedule)).ReturnsAsync(true);
        _mockAppointmentService.Setup(a => a.RejectedAppointment(1)).ReturnsAsync(true);

        // Act
        var result = await _service.RejectedSchedule(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Rejected MedicalSchedule success", result.Message);
    }

    [Fact]
    public async Task RejectedSchedule_ShouldReturnError_WhenUpdateFails()
    {
        // Arrange
        var schedule = new MedicalScheduleModel { Id = 1, Status = "Busy", AppointmentId = 1 };
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.GetByIdAsync(1)).ReturnsAsync(schedule);
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.UpdateAsync(schedule)).ReturnsAsync(false);
        _mockAppointmentService.Setup(a => a.RejectedAppointment(1)).ReturnsAsync(false);

        // Act
        var result = await _service.RejectedSchedule(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Rejected MedicalSchedule", result.Message);
    }

    [Fact]
    public async Task Create_ShouldCreateMedicalSchedule_WhenValidRequest()
    {
        // Arrange
        var request = new MedicalScheduleRequest { DoctorId = 1, SpecialtyId = 1, Date = DateTime.Now, StartTime = new TimeSpan(08,00,00,31900), EndTime = new TimeSpan(08, 50, 00, 31900), Status = "Available", AppointmentId = null, MotiveCancellation = null };
        var model = new MedicalScheduleModel { DoctorId = 1, SpecialtyId = 1, Date = DateTime.Now, StartTime = new TimeSpan(08, 00, 00, 31900), EndTime = new TimeSpan(08, 50, 00, 31900), Status = "Available", AppointmentId = null, MotiveCancellation = null };

        _mockMapper.Setup(m => m.Map<MedicalScheduleModel>(request)).Returns(model);
        _mockUnitOfWork.Setup(u => u.MedicalScheduleRepository.InsertAsync(model)).ReturnsAsync(1);

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalSchedule Created", result.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var request = new MedicalScheduleRequest { /* Propriedades inválidas */ };

        _mockColetorErrors.Setup(c => c.GenerateErrors()).Returns(new List<string> { "Validation Error" });

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Creating MedicalSchedule", result.Message);
    }
}