using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Application.Services;
using dhbw.WebEngineering.V2.Domain.Building;
using Moq;
using Xunit;

namespace dhbw.WebEngineering.V2.Tests.UnitTests;

public class BuildingServiceTests
{
    private readonly Mock<IBuildingRepository> _mockBuildingRepository;
    private readonly BuildingService _buildingService;

    public BuildingServiceTests()
    {
        _mockBuildingRepository = new Mock<IBuildingRepository>();
        _buildingService = new BuildingService(_mockBuildingRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnBuildings_WhenBuildingsExist()
    {
        // Arrange
        var buildings = new List<Building>
        {
            Building.Create("Building1", "Street1", "123", "US", "12345", "City1"),
            Building.Create("Building2", "Street2", "456", "US", "54321", "City2"),
        };

        _mockBuildingRepository
            .Setup(repo => repo.GetAllAsync(false))
            .ReturnsAsync(Maybe<List<Building>>.From(buildings));

        // Act
        var result = await _buildingService.GetAllAsync(false);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnFailure_WhenNoBuildingsExist()
    {
        // Arrange
        _mockBuildingRepository
            .Setup(repo => repo.GetAllAsync(false))
            .ReturnsAsync(Maybe<List<Building>>.None);

        // Act
        var result = await _buildingService.GetAllAsync(false);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("No Buildings found", result.Error);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnBuilding_WhenBuildingExists()
    {
        // Arrange
        var building = Building.Create("Building1", "Street1", "123", "US", "12345", "City1");

        _mockBuildingRepository
            .Setup(repo => repo.GetByIdAsync(building.id))
            .ReturnsAsync(Maybe<Building>.From(building));

        // Act
        var result = await _buildingService.GetByIdAsync(building.id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(building.id, result.Value.id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenBuildingDoesNotExist()
    {
        // Arrange
        var buildingId = Guid.NewGuid();

        _mockBuildingRepository
            .Setup(repo => repo.GetByIdAsync(buildingId))
            .ReturnsAsync(Maybe<Building>.None);

        // Act
        var result = await _buildingService.GetByIdAsync(buildingId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"No existing Building with the Id: {buildingId}", result.Error);
    }

    [Fact]
    public async Task CreateNewAsync_ShouldReturnSuccess_WhenBuildingIsCreated()
    {
        // Arrange
        var building = Building.Create("Building1", "Street1", "123", "US", "12345", "City1");

        _mockBuildingRepository
            .Setup(repo => repo.CreateAsync(building))
            .ReturnsAsync(Maybe<Building>.From(building));

        // Act
        var result = await _buildingService.CreateNewAsync(building);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(building.id, result.Value.id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnSuccess_WhenBuildingIsUpdated()
    {
        // Arrange
        var buildingId = Guid.NewGuid();
        var updatedBuilding = Building.Create(
            "UpdatedBuilding",
            "UpdatedStreet",
            "456",
            "US",
            "54321",
            "UpdatedCity"
        );

        _mockBuildingRepository
            .Setup(repo => repo.UpdateAsync(updatedBuilding, buildingId))
            .ReturnsAsync(Maybe<Building>.From(updatedBuilding));

        // Act
        var result = await _buildingService.UpdateAsync(updatedBuilding, buildingId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(updatedBuilding.name, result.Value.name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenBuildingIsDeleted()
    {
        // Arrange
        var buildingId = Guid.NewGuid();

        _mockBuildingRepository
            .Setup(repo => repo.DeleteAsync(buildingId, false))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _buildingService.DeleteAsync(buildingId, false);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailure_WhenBuildingCannotBeDeleted()
    {
        // Arrange
        var buildingId = Guid.NewGuid();

        _mockBuildingRepository
            .Setup(repo => repo.DeleteAsync(buildingId, false))
            .ReturnsAsync(Result.Failure("Cant delete Building with active Storey"));

        // Act
        var result = await _buildingService.DeleteAsync(buildingId, false);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Cant delete Building with active Storey", result.Error);
    }
}
