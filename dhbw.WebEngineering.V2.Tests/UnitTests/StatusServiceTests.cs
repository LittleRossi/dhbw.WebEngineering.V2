namespace dhbw.WebEngineering.V2.Tests.UnitTests;

using System.Collections.Generic;
using Application.Services;
using Domain.Entities.Status;
using Domain.Interfaces.Repository;
using Moq;
using Xunit;

public class StatusServiceTests
{
    private readonly Mock<IStatusRepository> _statusRepositoryMock;
    private readonly StatusService _statusService;

    public StatusServiceTests()
    {
        _statusRepositoryMock = new Mock<IStatusRepository>();
        _statusService = new StatusService(_statusRepositoryMock.Object);
    }

    [Fact]
    public void GetStatusInformation_ShouldReturnStatusInformation_WhenRepositoryReturnsData()
    {
        // Arrange
        var expectedStatusInformation = new StatusInformation
        {
            authors = new List<string> { "Nick Starzmann", "Mario Grimm", "Andreas Bauer" },
            supportedApis = new List<string> { "jwt-v2", "assets-v3", "reservations-v2" },
        };

        _statusRepositoryMock.Setup(repo => repo.GetStatus()).Returns(expectedStatusInformation);

        // Act
        var result = _statusService.GetStatusInformation();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedStatusInformation.authors, result.authors);
        Assert.Equal(expectedStatusInformation.supportedApis, result.supportedApis);
    }

    [Fact]
    public void GetStatusInformation_ShouldHandleEmptyAuthorsAndApisLists()
    {
        // Arrange
        var expectedStatusInformation = new StatusInformation
        {
            authors = new List<string>(),
            supportedApis = new List<string>(),
        };

        _statusRepositoryMock.Setup(repo => repo.GetStatus()).Returns(expectedStatusInformation);

        // Act
        var result = _statusService.GetStatusInformation();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.authors);
        Assert.Empty(result.supportedApis);
    }

    [Fact]
    public void GetStatusInformation_ShouldHandleNullListsSafely()
    {
        // Arrange
        var expectedStatusInformation = new StatusInformation
        {
            authors = null,
            supportedApis = null,
        };

        _statusRepositoryMock.Setup(repo => repo.GetStatus()).Returns(expectedStatusInformation);

        // Act
        var result = _statusService.GetStatusInformation();

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.authors);
        Assert.Null(result.supportedApis);
    }
}
