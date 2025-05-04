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

public class MedicalServiceServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IColetorErrors> _mockColetorErrors;
    private readonly MedicalServiceService _service;

    public MedicalServiceServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockColetorErrors = new Mock<IColetorErrors>();

        _service = new MedicalServiceService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockColetorErrors.Object
        );
    }

    [Fact]
    public async Task Create_ShouldCreateMedicalService_WhenValidRequest()
    {
        // Arrange
        var request = new MedicalServiceRequest { Name = "Consulta Dermatológica", IdMedicalEspecialty = 1 };
        var model = new MedicalServiceModel { Name = "Consulta Dermatológica", IdMedicalEspecialty = 1, Active = true };

        _mockMapper.Setup(m => m.Map<MedicalServiceModel>(request)).Returns(model);
        _mockUnitOfWork.Setup(u => u.MedicalServiceRepository.InsertAsync(model)).ReturnsAsync(1);

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalService Created", result.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var request = new MedicalServiceRequest { /* Propriedades inválidas */ };

        _mockColetorErrors.Setup(c => c.GenerateErrors()).Returns(new List<string> { "Validation Error" });

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Creating MedicalService", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldDeleteMedicalService_WhenMedicalServiceExists()
    {
        // Arrange
        var medicalService = new MedicalServiceModel { Id = 1 };

        _mockUnitOfWork.Setup(u => u.MedicalServiceRepository.GetByIdAsync(1)).ReturnsAsync(medicalService);
        _mockUnitOfWork.Setup(u => u.MedicalServiceRepository.DisableAsync(medicalService)).ReturnsAsync(true);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalService Deleted", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnError_WhenMedicalServiceNotFound()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.MedicalServiceRepository.GetByIdAsync(1)).ReturnsAsync((MedicalServiceModel?)null);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("MedicalService not found", result.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMedicalServices()
    {
        // Arrange
        var medicalServices = new List<MedicalServiceModel>
        {
            new MedicalServiceModel { Id = 1 },
            new MedicalServiceModel { Id = 2 }
        };
        var responses = new List<MedicalServiceResponse>
        {
            new MedicalServiceResponse { Id = 1 },
            new MedicalServiceResponse { Id = 2 }
        };

        _mockUnitOfWork.Setup(u => u.MedicalServiceRepository.GetAllAsync()).ReturnsAsync(medicalServices);
        _mockMapper.Setup(m => m.Map<IEnumerable<MedicalServiceResponse>>(medicalServices)).Returns(responses);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Data?.Count());
    }

    [Fact]
    public async Task GetById_ShouldReturnMedicalService_WhenMedicalServiceExists()
    {
        // Arrange
        var medicalService = new MedicalServiceModel { Id = 1 };
        var response = new MedicalServiceResponse { Id = 1 };

        _mockUnitOfWork.Setup(u => u.MedicalServiceRepository.GetByIdAsync(1)).ReturnsAsync(medicalService);
        _mockMapper.Setup(m => m.Map<MedicalServiceResponse>(medicalService)).Returns(response);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalService Found", result.Message);
    }

    [Fact]
    public async Task Update_ShouldUpdateMedicalService_WhenValidRequest()
    {
        // Arrange
        var request = new MedicalServiceRequest { Name = "teste 2", IdMedicalEspecialty = 1};
        var model = new MedicalServiceModel { Name = "teste 2", IdMedicalEspecialty = 1, Active = true };

        _mockMapper.Setup(m => m.Map<MedicalServiceModel>(request)).Returns(model);
        _mockUnitOfWork.Setup(u => u.MedicalServiceRepository.UpdateAsync(model)).ReturnsAsync(true);

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalService Updated", result.Message);
    }
}
