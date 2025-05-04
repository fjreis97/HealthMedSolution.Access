using AutoMapper;
using Health_Med.Business.Interfaces;
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

public class CollaboratorServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IColetorErrors> _mockColetorErrors;
    private readonly Mock<IDoctorService> _mockDoctorService;
    private readonly Mock<IPasswordGenerate> _mockPasswordGenerate;
    private readonly CollaboratorService _service;

    public CollaboratorServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockColetorErrors = new Mock<IColetorErrors>();
        _mockDoctorService = new Mock<IDoctorService>();
        _mockPasswordGenerate = new Mock<IPasswordGenerate>();

        _service = new CollaboratorService(
            _mockUnitOfWork.Object,
            _mockMapper.Object,
            _mockColetorErrors.Object,
            _mockDoctorService.Object,
            _mockPasswordGenerate.Object
        );
    }

    [Fact]
    public async Task Create_ShouldCreateCollaborator_WhenValidRequest()
    {
        // Arrange
        var request = new CollaboratorRequest { Name = "Jureminha silva", EmailAddress = "Jureminha.silva@example.com", DateOfBirth = new DateTime(2000,04,12), Cpf = "90354193082", Rg = "328426222", DateOfAdmission = new DateTime(2025,01,01),  Password = "Senha@123", ConfirmPassword = "Senha@123", RoleId = 1 };
        var hashedPassword = "$2a$11$1L6JCavnAJXvHAmnglPGqu7KlzIXAQx6OiK4O7sidvsURUUQcRPx.";

        _mockPasswordGenerate.Setup(p => p.GeneratePasswordHash(request.Password)).Returns(hashedPassword);
        _mockUnitOfWork.Setup(u => u.CollaboratorRepository.InsertAsync(It.IsAny<CollaboratorModel>())).ReturnsAsync(1);

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Collaborator Created", result.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var request = new CollaboratorRequest { /* Propriedades inválidas */ };

        _mockColetorErrors.Setup(c => c.GenerateErrors()).Returns(new List<string> { "Validation Error" });

        // Act
        var result = await _service.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Error Creating Collaborator", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldDeleteCollaborator_WhenValidId()
    {
        // Arrange
        var collaborator = new CollaboratorModel { Id = 1 };

        _mockUnitOfWork.Setup(u => u.CollaboratorRepository.GetByIdAsync(1)).ReturnsAsync(collaborator);
        _mockUnitOfWork.Setup(u => u.CollaboratorRepository.DisableAsync(collaborator)).ReturnsAsync(true);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal("Collaborator Deleted", result.Message);
    }

    [Fact]
    public async Task Delete_ShouldReturnError_WhenCollaboratorNotFound()
    {
        // Arrange
        _mockUnitOfWork.Setup(u => u.CollaboratorRepository.GetByIdAsync(1)).ReturnsAsync((CollaboratorModel?)null);

        // Act
        var result = await _service.Delete(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Collaborator not found", result.Message);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnCollaborators()
    {
        // Arrange
        var collaborators = new List<CollaboratorModel>
    {
        new CollaboratorModel { Id = 1 },
        new CollaboratorModel { Id = 2 }
    };
        var responses = new List<CollaboratorResponse>
    {
        new CollaboratorResponse { Id = 1 },
        new CollaboratorResponse { Id = 2 }
    };

        _mockUnitOfWork.Setup(u => u.CollaboratorRepository.GetAllAsync()).ReturnsAsync(collaborators);
        _mockMapper.Setup(m => m.Map<IEnumerable<CollaboratorResponse>>(collaborators)).Returns(responses);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Data?.Count());
    }

    [Fact]
    public async Task VerifyExistence_ShouldReturnCollaborator_WhenEmailIsValid()
    {
        // Arrange
        var request = new LoginRequest { Input = "test@example.com", Password = "password123" };
        var collaborator = new CollaboratorModel { EmailAddress = "test@example.com", Password = "hashedPassword123" };

        _mockUnitOfWork.Setup(u => u.CollaboratorRepository.GetAllAsync()).ReturnsAsync(new List<CollaboratorModel> { collaborator });
        _mockPasswordGenerate.Setup(p => p.ValidadePassword(request.Password, collaborator.Password)).Returns(true);
        _mockMapper.Setup(m => m.Map<CollaboratorResponse>(collaborator)).Returns(new CollaboratorResponse { EmailAddress = "test@example.com" });

        // Act
        var result = await _service.VerifyExistence(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@example.com", result.EmailAddress);
    }
}
