using dhbw.WebEngineering.V2.Domain.Storey;

namespace dhbw.WebEngineering.V2.Tests.UnitTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services;
using CSharpFunctionalExtensions;
using Moq;
using Xunit;

public class StoreyServiceTests
{
    private readonly Mock<IStoreyRepository> _storeyRepositoryMock;
    private readonly StoreyService _storeyService;

    public StoreyServiceTests()
    {
        _storeyRepositoryMock = new Mock<IStoreyRepository>();
        _storeyService = new StoreyService(_storeyRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateNewAsync_ShouldReturnSuccessResult_WhenRepositorySucceeds()
    {
        // Arrange
        var newStorey = Storey.Create("First Floor", Guid.NewGuid()).Value;

        _storeyRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Storey>()))
            .ReturnsAsync(Maybe<Storey>.From(newStorey));

        // Act
        var result = await _storeyService.CreateNewAsync(newStorey);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newStorey, result.Value);
        _storeyRepositoryMock.Verify(repo => repo.CreateAsync(newStorey), Times.Once);
    }

    [Fact]
    public async Task CreateNewAsync_ShouldReturnFailureResult_WhenRepositoryFails()
    {
        // Arrange
        var newStorey = Storey.Create("Second Floor", Guid.NewGuid()).Value;

        _storeyRepositoryMock
            .Setup(repo => repo.CreateAsync(It.IsAny<Storey>()))
            .ReturnsAsync(Maybe<Storey>.None);

        // Act
        var result = await _storeyService.CreateNewAsync(newStorey);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("An Error happened while trying to Create a Storey", result.Error);
        _storeyRepositoryMock.Verify(repo => repo.CreateAsync(newStorey), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStoreys_WhenNoBuildingIdIsProvided()
    {
        // Arrange
        var storeys = new List<Storey>
        {
            Storey.Create("First Floor", Guid.NewGuid()).Value,
            Storey.Create("Second Floor", Guid.NewGuid()).Value,
        };

        _storeyRepositoryMock
            .Setup(repo => repo.GetAllAsync(false))
            .ReturnsAsync(Maybe<List<Storey>>.From(storeys));

        // Act
        var result = await _storeyService.GetAllAsync(null);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(storeys, result.Value);
        _storeyRepositoryMock.Verify(repo => repo.GetAllAsync(false), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldFilterStoreysByBuildingId_WhenBuildingIdIsProvided()
    {
        // Arrange
        var buildingId = Guid.NewGuid();
        var storeys = new List<Storey>
        {
            Storey.Create("First Floor", buildingId).Value,
            Storey.Create("Second Floor", Guid.NewGuid()).Value,
        };

        _storeyRepositoryMock
            .Setup(repo => repo.GetAllAsync(false))
            .ReturnsAsync(Maybe<List<Storey>>.From(storeys));

        // Act
        var result = await _storeyService.GetAllAsync(buildingId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.Equal(buildingId, result.Value.First().building_id);
        _storeyRepositoryMock.Verify(repo => repo.GetAllAsync(false), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnStorey_WhenStoreyExists()
    {
        // Arrange
        var storeyId = Guid.NewGuid();
        var storey = Storey.Create("First Floor", Guid.NewGuid()).Value;

        _storeyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(storeyId))
            .ReturnsAsync(Maybe<Storey>.From(storey));

        // Act
        var result = await _storeyService.GetByIdAsync(storeyId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(storey, result.Value);
        _storeyRepositoryMock.Verify(repo => repo.GetByIdAsync(storeyId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenStoreyDoesNotExist()
    {
        // Arrange
        var storeyId = Guid.NewGuid();

        _storeyRepositoryMock
            .Setup(repo => repo.GetByIdAsync(storeyId))
            .ReturnsAsync(Maybe<Storey>.None);

        // Act
        var result = await _storeyService.GetByIdAsync(storeyId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"No existing Storey with ID: {storeyId}", result.Error);
        _storeyRepositoryMock.Verify(repo => repo.GetByIdAsync(storeyId), Times.Once);
    }
}
