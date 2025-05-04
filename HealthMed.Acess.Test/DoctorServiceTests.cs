using AutoMapper;
using Health_Med.Business;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Dtos.Response;
using Health_Med.Domain.Model;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Acess.Test;

public class DoctorServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IColetorErrors> _mockColetorErrors;
    private readonly DoctorService _service;

    public DoctorServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockColetorErrors = new Mock<IColetorErrors>();

        _service = new DoctorService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockColetorErrors.Object
        );
    }

    [Fact]
    public async Task Create_ShouldCreateDoctor_WhenValidRequest()
    {
        // Arrange
        var request = new DoctorRequest { Id = 1, Crm = "123456-SP", Rqe = "108763", IdCollaborator = 4 };
        var doctorModel = new DoctorModel { Id = 1, Crm = "123456-SP", Rqe = "108763", IdCollaborator = 4 };

        _mockMapper.Setup(m => m.Map<DoctorModel>(request)).Returns(doctorModel);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.InsertAsync(doctorModel)).ReturnsAsync(1);

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Doctor Created", result.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var request = new DoctorRequest {Crm = "125", IdCollaborator = 999, Rqe = "teste" };

        _mockColetorErrors.Setup(c => c.GenerateErrors()).Returns(new List<string> { "Validation Error" });

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Creating Doctor", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldDeleteDoctor_WhenDoctorExists()
    {
        // Arrange
        var doctor = new DoctorModel { Id = 1 };

        _mockUnitOfWork.Setup(u => u.DoctorRepository.GetByIdAsync(1)).ReturnsAsync(doctor);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.DisableAsync(doctor)).ReturnsAsync(true);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Doctor Deleted", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnError_WhenDoctorNotFound()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.DoctorRepository.GetByIdAsync(1)).ReturnsAsync((DoctorModel?)null);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Doctor not found", result.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnDoctors()
    {
        // Arrange
        var doctors = new List<DoctorModel> { new DoctorModel { Id = 1 }, new DoctorModel { Id = 2 } };
        var doctorResponses = new List<DoctorResponse> { new DoctorResponse { Id = 1 }, new DoctorResponse { Id = 2 } };

        _mockUnitOfWork.Setup(u => u.DoctorRepository.GetAllAsync()).ReturnsAsync(doctors);
        _mockMapper.Setup(m => m.Map<IEnumerable<DoctorResponse>>(doctors)).Returns(doctorResponses);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Data?.Count());
    }

    [Fact]
    public async Task GetById_ShouldReturnDoctor_WhenDoctorExists()
    {
        // Arrange
        var doctor = new DoctorModel { Id = 1 };
        var doctorResponse = new DoctorResponse { Id = 1 };

        _mockUnitOfWork.Setup(u => u.DoctorRepository.GetByIdAsync(1)).ReturnsAsync(doctor);
        _mockMapper.Setup(m => m.Map<DoctorResponse>(doctor)).Returns(doctorResponse);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Doctor Found", result.Message);
    }

    [Fact]
    public async Task Update_ShouldUpdateDoctor_WhenValidRequest()
    {
        // Arrange
        var request = new DoctorRequest { Id = 1, IdCollaborator = 4, Rqe = "108763", Crm = "123456-SP" };
        var doctorModel = new DoctorModel { Id = 1, IdCollaborator = 4, Rqe = "108763", Crm = "123456-SP" };

        _mockMapper.Setup(m => m.Map<DoctorModel>(request)).Returns(doctorModel);
        _mockUnitOfWork.Setup(u => u.DoctorRepository.UpdateAsync(doctorModel)).ReturnsAsync(true);

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Doctor Updated", result.Message);
    }
}
