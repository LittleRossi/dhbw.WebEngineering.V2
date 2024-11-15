using CSharpFunctionalExtensions;
using dhbw.WebEngineering.V2.Application.Services;
using dhbw.WebEngineering.V2.Domain.Room;
using Moq;
using Xunit;

namespace dhbw.WebEngineering.V2.Tests.UnitTests;

public class RoomServiceTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly RoomService _roomService;

    public RoomServiceTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _roomService = new RoomService(_roomRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRooms_WhenNoStoreyIdProvided()
    {
        // Arrange
        var rooms = new List<Room>
        {
            Room.Create("Room1", Guid.NewGuid()).Value,
            Room.Create("Room2", Guid.NewGuid()).Value,
        };

        _roomRepositoryMock
            .Setup(repo => repo.GetAllAsync(false))
            .ReturnsAsync(Maybe<List<Room>>.From(rooms));

        // Act
        var result = await _roomService.GetAllAsync(null);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(rooms, result.Value);
    }

    [Fact]
    public async Task GetAllAsync_ShouldFilterRoomsByStoreyId_WhenStoreyIdProvided()
    {
        // Arrange
        var storeyId = Guid.NewGuid();
        var rooms = new List<Room>
        {
            Room.Create("Room1", storeyId).Value,
            Room.Create("Room2", Guid.NewGuid()).Value,
        };

        _roomRepositoryMock
            .Setup(repo => repo.GetAllAsync(false))
            .ReturnsAsync(Maybe<List<Room>>.From(rooms));

        // Act
        var result = await _roomService.GetAllAsync(storeyId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.Equal(storeyId, result.Value[0].storey_id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRoom_WhenRoomExists()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = Room.Create("Room1", Guid.NewGuid()).Value;
        room = room with { id = roomId }; // Adjust the ID for the test

        _roomRepositoryMock
            .Setup(repo => repo.GetByIdAsync(roomId))
            .ReturnsAsync(Maybe<Room>.From(room));

        // Act
        var result = await _roomService.GetByIdAsync(roomId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(room, result.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenRoomDoesNotExist()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(roomId)).ReturnsAsync(Maybe<Room>.None);

        // Act
        var result = await _roomService.GetByIdAsync(roomId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal($"No existing Room with ID: {roomId}", result.Error);
    }

    [Fact]
    public async Task CreateNewAsync_ShouldReturnRoom_WhenCreationSucceeds()
    {
        // Arrange
        var room = Room.Create("Room1", Guid.NewGuid()).Value;

        _roomRepositoryMock
            .Setup(repo => repo.CreateAsync(room))
            .ReturnsAsync(Maybe<Room>.From(room));

        // Act
        var result = await _roomService.CreateNewAsync(room);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(room, result.Value);
    }

    [Fact]
    public async Task CreateNewAsync_ShouldReturnFailure_WhenCreationFails()
    {
        // Arrange
        var room = Room.Create("Room1", Guid.NewGuid()).Value;

        _roomRepositoryMock.Setup(repo => repo.CreateAsync(room)).ReturnsAsync(Maybe<Room>.None);

        // Act
        var result = await _roomService.CreateNewAsync(room);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("An Error happened while trying to Create a Room", result.Error);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnRoom_WhenUpdateSucceeds()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = Room.Create("UpdatedRoom", Guid.NewGuid()).Value;
        room = room with { id = roomId }; // Adjust the ID for the test

        _roomRepositoryMock
            .Setup(repo => repo.UpdateAsync(room, roomId))
            .ReturnsAsync(Maybe<Room>.From(room));

        // Act
        var result = await _roomService.UpdateAsync(room, roomId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(room, result.Value);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFailure_WhenUpdateFails()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var room = Room.Create("UpdatedRoom", Guid.NewGuid()).Value;

        _roomRepositoryMock
            .Setup(repo => repo.UpdateAsync(room, roomId))
            .ReturnsAsync(Maybe<Room>.None);

        // Act
        var result = await _roomService.UpdateAsync(room, roomId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("An Error happened while trying to Update a Room", result.Error);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnSuccess_WhenDeleteSucceeds()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _roomRepositoryMock
            .Setup(repo => repo.DeleteAsync(roomId, false))
            .ReturnsAsync(Result.Success());

        // Act
        var result = await _roomService.DeleteAsync(roomId);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFailure_WhenDeleteFails()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        _roomRepositoryMock
            .Setup(repo => repo.DeleteAsync(roomId, false))
            .ReturnsAsync(Result.Failure("Delete failed"));

        // Act
        var result = await _roomService.DeleteAsync(roomId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Delete failed", result.Error);
    }
}
