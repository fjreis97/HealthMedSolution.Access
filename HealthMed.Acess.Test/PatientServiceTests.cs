using Health_Med.Business.Interfaces;
using Health_Med.Business;
using Health_Med.Infrastructure.UnitOfWork.Interface;
using HealthMed.API.Access.Common.ColetorErrors.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Health_Med.Domain.Dtos.Request;
using Health_Med.Domain.Model;
using Health_Med.Domain.Dtos.Response;
using HealthMed.API.Access.Common.ResponseDefault;

namespace HealthMed.Acess.Test;

public class PatientServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IColetorErrors> _mockColetorErrors;
    private readonly Mock<IPasswordGenerate> _mockPasswordGenerate;
    private readonly Mock<IPasswordGenerate> _mockHash;
    private readonly PatientService _service;

    public PatientServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockColetorErrors = new Mock<IColetorErrors>();
        _mockPasswordGenerate = new Mock<IPasswordGenerate>();
        _mockHash = new Mock<IPasswordGenerate>();

        _service = new PatientService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockColetorErrors.Object,
            _mockPasswordGenerate.Object,
            _mockHash.Object
        );
    }

    //TODO: AJUSTAR
    //[Fact]
    //public async Task Create_ShouldReturnSuccess_WhenPatientIsValid()
    //{
    //    // Arrange
    //    var request = new PatientRequest { Password = "Senha@123", ConfirmPassword = "Senha@123", Cpf = "15259644026", Rg = "112233445", EmailAddress = "joao.silva@fiap.com", DateOfRegistration = DateTime.Now, RoleId = 4 };
    //    var patientModel = new PatientModel{Active=true,  Password = "Senha@123", ConfirmPassword = "Senha@123", Cpf = "15259644026", Rg = "112233445", EmailAddress = "joao.silva@fiap.com", DateOfRegistration = DateTime.Now, RoleId = 4 };
    //    _mockHash.Setup(h => h.GeneratePasswordHash(It.IsAny<string>())).Returns("hashedPassword");
    //    _mockMapper.Setup(m => m.Map<PatientModel>(request)).Returns(patientModel);
    //    _mockUnitOfWork.Setup(u => u.PatientRepository.InsertAsync(patientModel)).ReturnsAsync(1L);

    //    // Act
    //    var result = await _service.Create(request);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.True(result.IsSuccess);
    //    Assert.Equal("Patient Created", result.Message);
    //}

    [Fact]
    public async Task Create_ShouldReturnError_WhenInsertFails()
    {
        // Arrange
        var request = new PatientRequest { Password = "password123" };
        var patientModel = new PatientModel();
        _mockHash.Setup(h => h.GeneratePasswordHash(It.IsAny<string>())).Returns("hashedPassword");
        _mockMapper.Setup(m => m.Map<PatientModel>(request)).Returns(patientModel);
        _mockUnitOfWork.Setup(u => u.PatientRepository.InsertAsync(patientModel)).ReturnsAsync(0L);

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Creating Patient", result.Message);
    }


    [Fact]
    public async Task Delete_ShouldReturnSuccess_WhenPatientExists()
    {
        // Arrange
        var patient = new PatientModel { Id = 1 };
        _mockUnitOfWork.Setup(u => u.PatientRepository.GetByIdAsync(1)).ReturnsAsync(patient);
        _mockUnitOfWork.Setup(u => u.PatientRepository.DisableAsync(patient)).ReturnsAsync(true);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Patient Deleted", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnError_WhenPatientDoesNotExist()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.PatientRepository.GetByIdAsync(1)).ReturnsAsync((PatientModel)null);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Patient not found", result.Message);
    }

    [Fact]
    public async Task GetById_ShouldReturnPatient_WhenPatientExists()
    {
        // Arrange
        var patient = new PatientModel { Id = 1 };
        var patientResponse = new PatientResponse { Id = 1 };
        _mockUnitOfWork.Setup(u => u.PatientRepository.GetByIdAsync(1)).ReturnsAsync(patient);
        _mockMapper.Setup(m => m.Map<PatientResponse>(patient)).Returns(patientResponse);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Patient Found", result.Message);
        Assert.Equal(patientResponse, result.Data);
    }

    [Fact]
    public async Task GetById_ShouldReturnError_WhenPatientDoesNotExist()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.PatientRepository.GetByIdAsync(1)).ReturnsAsync((PatientModel)null);

        // Act
        var result = await _service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Patient not found", result.Message);
    }

    //TODO: AJUSTAR
    //[Fact]
    //public async Task Update_ShouldReturnSuccess_WhenUpdateIsSuccessful()
    //{
    //    // Arrange
    //    var request = new PatientRequest();
    //    var patientModel = new PatientModel();
    //    _mockMapper.Setup(m => m.Map<PatientModel>(request)).Returns(patientModel);
    //    _mockUnitOfWork.Setup(u => u.PatientRepository.UpdateAsync(patientModel)).ReturnsAsync(true);

    //    // Act
    //    var result = await _service.Update(request);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.True(result.IsSuccess);
    //    Assert.Equal("Patient Updated", result.Message);
    //}

    [Fact]
    public async Task Update_ShouldReturnError_WhenUpdateFails()
    {
        // Arrange
        var request = new PatientRequest();
        var patientModel = new PatientModel();
        _mockMapper.Setup(m => m.Map<PatientModel>(request)).Returns(patientModel);
        _mockUnitOfWork.Setup(u => u.PatientRepository.UpdateAsync(patientModel)).ReturnsAsync(false);

        // Act
        var result = await _service.Update(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Updating Patient", result.Message);
    }


}
