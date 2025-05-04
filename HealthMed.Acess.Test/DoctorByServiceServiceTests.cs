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

public class DoctorByServiceServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IColetorErrors> _mockColetorErrors;
    private readonly DoctorByServiceService _service;

    public DoctorByServiceServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockColetorErrors = new Mock<IColetorErrors>();

        _service = new DoctorByServiceService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockColetorErrors.Object
        );
    }

    [Fact]
    public async Task Create_ShouldCreateDoctorByService_WhenValidRequest()
    {
        // Arrange
        var request = new DoctorByServiceRequest { IdDoctor = 1, IdService = 1 };
        var model = new DoctorByServiceModel { IdDoctor = 1, IdService = 1  };

        _mockMapper.Setup(m => m.Map<DoctorByServiceModel>(request)).Returns(model);
        _mockUnitOfWork.Setup(u => u.DoctorByServiceRepository.InsertAsync(model)).ReturnsAsync(1);

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("DoctorByService Created", result.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var request = new DoctorByServiceRequest { /* Propriedades inválidas */ };

        _mockColetorErrors.Setup(c => c.GenerateErrors()).Returns(new List<string> { "Validation Error" });

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Creating DoctorByService", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldDeleteDoctorByService_WhenValidId()
    {
        // Arrange
        var doctorByService = new DoctorByServiceModel { IdDoctor = 1 };

        _mockUnitOfWork.Setup(u => u.DoctorByServiceRepository.GetByIdAsync(1)).ReturnsAsync(doctorByService);
        _mockUnitOfWork.Setup(u => u.DoctorByServiceRepository.DisableAsync(doctorByService)).ReturnsAsync(true);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("DoctorByService Deleted", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnError_WhenDoctorByServiceNotFound()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.DoctorByServiceRepository.GetByIdAsync(1)).ReturnsAsync((DoctorByServiceModel?)null);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("DoctorByService not found", result.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnDoctorByServices()
    {
        // Arrange
        var doctorByServices = new List<DoctorByServiceModel>
        {
            new DoctorByServiceModel { IdDoctor = 1 },
            new DoctorByServiceModel { IdDoctor = 2 }
        };
        var responses = new List<DoctorByServiceResponse>
        {
            new DoctorByServiceResponse { IdDoctor = 1 },
            new DoctorByServiceResponse { IdDoctor = 2 }
        };

        _mockUnitOfWork.Setup(u => u.DoctorByServiceRepository.GetAllAsync()).ReturnsAsync(doctorByServices);
        _mockMapper.Setup(m => m.Map<IEnumerable<DoctorByServiceResponse>>(doctorByServices)).Returns(responses);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Data?.Count());
    }

    [Fact]
    public async Task GetById_ShouldReturnDoctorByService_WhenValidId()
    {
        // Arrange
        var doctorByService = new DoctorByServiceModel { IdDoctor = 1 };
        var response = new DoctorByServiceResponse { IdDoctor = 1 };

        _mockUnitOfWork.Setup(u => u.DoctorByServiceRepository.GetByIdAsync(1)).ReturnsAsync(doctorByService);
        _mockMapper.Setup(m => m.Map<DoctorByServiceResponse>(doctorByService)).Returns(response);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("DoctorByService Found", result.Message);
    }

    [Fact]
    public async Task Update_ShouldUpdateDoctorByService_WhenValidRequest()
    {
        // Arrange
        var request = new DoctorByServiceRequest { IdDoctor = 1, IdService = 1 };
        var model = new DoctorByServiceModel { IdDoctor = 1, IdService = 1 };

        _mockMapper.Setup(m => m.Map<DoctorByServiceModel>(request)).Returns(model);
        _mockUnitOfWork.Setup(u => u.DoctorByServiceRepository.UpdateAsync(model)).ReturnsAsync(true);

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("DoctorByService Updated", result.Message);
    }
}
