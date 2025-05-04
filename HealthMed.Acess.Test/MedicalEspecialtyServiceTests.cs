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

public class MedicalEspecialtyServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IColetorErrors> _mockColetorErrors;
    private readonly MedicalEspecialtyService _service;

    public MedicalEspecialtyServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockColetorErrors = new Mock<IColetorErrors>();

        _service = new MedicalEspecialtyService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockColetorErrors.Object
        );
    }

    [Fact]
    public async Task Create_ShouldCreateMedicalEspecialty_WhenValidRequest()
    {
        // Arrange
        var request = new MedicalEspecialtyRequest { Name = "Endocrino"};
        var model = new MedicalEspecialtyModel { Name = "Endocrino" };

        _mockMapper.Setup(m => m.Map<MedicalEspecialtyModel>(request)).Returns(model);
        _mockUnitOfWork.Setup(u => u.MedicalEspecialtyRepository.InsertAsync(model)).ReturnsAsync(1);

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalEspecialty Created", result.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var request = new MedicalEspecialtyRequest { /* Propriedades inválidas */ };

        _mockColetorErrors.Setup(c => c.GenerateErrors()).Returns(new List<string> { "Validation Error" });

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Creating MedicalEspecialty", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldDeleteMedicalEspecialty_WhenMedicalEspecialtyExists()
    {
        // Arrange
        var medicalEspecialty = new MedicalEspecialtyModel { Id = 1 };

        _mockUnitOfWork.Setup(u => u.MedicalEspecialtyRepository.GetByIdAsync(1)).ReturnsAsync(medicalEspecialty);
        _mockUnitOfWork.Setup(u => u.MedicalEspecialtyRepository.DisableAsync(medicalEspecialty)).ReturnsAsync(true);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalEspecialty Deleted", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnError_WhenMedicalEspecialtyNotFound()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.MedicalEspecialtyRepository.GetByIdAsync(1)).ReturnsAsync((MedicalEspecialtyModel?)null);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("MedicalEspecialty not found", result.Message);
    }

    //[Fact]
    //public async Task GetAllAsync_ShouldReturnMedicalEspecialtys()
    //{
    //    // Arrange
    //    var medicalEspecialtys = new List<MedicalEspecialtyModel>
    //    {
    //        new MedicalEspecialtyModel { Id = 1, Name = "Dermatologista", Active = true },
    //        new MedicalEspecialtyModel { Id = 2, Name = "Ortopedista", Active = true  }
    //    };
    //    var responses = new List<MedicalEspecialtyResponse>
    //    {
    //        new MedicalEspecialtyResponse { Id = 1, Name = "Dermatologista", Active = true },
    //        new MedicalEspecialtyResponse { Id = 2, Name = "Ortopedista", Active = true }
    //    };

    //    _mockUnitOfWork.Setup(u => u.MedicalEspecialtyRepository.GetAllAsync()).ReturnsAsync(medicalEspecialtys);
    //    _mockMapper.Setup(m => m.Map<IEnumerable<MedicalEspecialtyResponse>>(medicalEspecialtys)).Returns(responses);

    //    // Act
    //    var result = await _service.GetAllAsync();

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(2, result.Data?.Count());
    //}

    [Fact]
    public async Task GetById_ShouldReturnMedicalEspecialty_WhenMedicalEspecialtyExists()
    {
        // Arrange
        var medicalEspecialty = new MedicalEspecialtyModel { Id = 1 };
        var response = new MedicalEspecialtyResponse { Id = 1 };

        _mockUnitOfWork.Setup(u => u.MedicalEspecialtyRepository.GetByIdAsync(1)).ReturnsAsync(medicalEspecialty);
        _mockMapper.Setup(m => m.Map<MedicalEspecialtyResponse>(medicalEspecialty)).Returns(response);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalEspecialty Found", result.Message);
    }

    [Fact]
    public async Task Update_ShouldUpdateMedicalEspecialty_WhenValidRequest()
    {
        // Arrange
        var request = new MedicalEspecialtyRequest {Name = "Endocrinologista" };
        var model = new MedicalEspecialtyModel {Name = "Endocrinologista" };

        _mockMapper.Setup(m => m.Map<MedicalEspecialtyModel>(request)).Returns(model);
        _mockUnitOfWork.Setup(u => u.MedicalEspecialtyRepository.UpdateAsync(model)).ReturnsAsync(true);

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("MedicalEspecialty Updated", result.Message);
    }
}
