using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services.MiniGameService;


[TestFixture]
public class MiniGameServiceTests
{
    private MiniGameService _miniGameService;
    private List<FishNet.Connection.NetworkConnection> _players;

    [SetUp]
    public void SetUp()
    {
        // Этот метод выполняется перед каждым тестом
        _miniGameService = new MiniGameService();
        _players = new List<FishNet.Connection.NetworkConnection>
        {
            new FishNet.Connection.NetworkConnection(),
            new FishNet.Connection.NetworkConnection()
        };
    }

    // Тест 1: Успешное создание и добавление игры
    [Test]
    public void CreateMiniGame_ShouldCreateAndAddGame_WhenCalledWithValidParameters()
    {
        // Arrange
        var gameType = MiniGameType.DefaultGame;


        // Act
        var createdGame = _miniGameService.CreateMiniGame(gameType, _players);

        // Assert
        var activeGames = GetActiveGames(_miniGameService);
        
        Assert.AreEqual(1, activeGames.Count, "В словаре должна быть одна активная игра.");
        Assert.IsTrue(activeGames.ContainsKey(createdGame.InstanceId), "Словарь должен содержать игру с созданным ID.");
    }

    // Тест 2: Присвоение уникальных ID
    [Test]
    public void CreateMiniGame_ShouldAssignUniqueIds_WhenCreatingMultipleGames()
    {
        // Arrange
        var gameType = MiniGameType.DefaultGame;

        // Act
        var game1 = _miniGameService.CreateMiniGame(gameType, _players);
        var game2 = _miniGameService.CreateMiniGame(gameType, _players);

        // Assert
        Assert.AreNotEqual(game1.InstanceId, game2.InstanceId, "ID двух созданных игр не должны совпадать.");
        
        var activeGames = GetActiveGames(_miniGameService);
        Assert.AreEqual(2, activeGames.Count, "В словаре должно быть две активные игры.");
    }

    // Тест 3: Корректное удаление существующей игры
    [Test]
    public void EndMiniGame_ShouldRemoveGame_WhenGameExists()
    {
        // Arrange
        var createdGame = _miniGameService.CreateMiniGame(MiniGameType.DefaultGame, _players);
        var gameId = createdGame.InstanceId;
        var activeGames = GetActiveGames(_miniGameService);
        
        Assert.AreEqual(1, activeGames.Count, "Игра должна быть добавлена перед удалением.");

        // Act
        _miniGameService.EndMiniGame(gameId);

        // Assert
        Assert.AreEqual(0, activeGames.Count, "Игра должна была быть удалена из словаря.");
        Assert.IsFalse(activeGames.ContainsKey(gameId), "В словаре не должно быть ключа удаленной игры.");
    }
    
    // Тест 4: Попытка удалить несуществующую игру
    [Test]
    public void EndMiniGame_ShouldDoNothing_WhenGameDoesNotExist()
    {
        // Arrange
        int nonExistentId = 999;
        _miniGameService.CreateMiniGame(MiniGameType.DefaultGame, _players);

        // Act & Assert
        Assert.DoesNotThrow(() => _miniGameService.EndMiniGame(nonExistentId));
        var activeGames = GetActiveGames(_miniGameService);
        Assert.AreEqual(1, activeGames.Count, "Количество игр не должно было измениться.");
    }

    // Тест 5: Метод создания возвращает корректный экземпляр
    [Test]
    public void CreateMiniGame_ShouldReturnCorrectGameInstance()
    {
        // Arrange
        var gameType = MiniGameType.AnotherGame;
        
        // Act
        var createdGame = _miniGameService.CreateMiniGame(gameType, _players);
        
        // Assert
        Assert.IsNotNull(createdGame, "Созданный объект игры не должен быть null.");
        Assert.AreEqual(gameType, createdGame.GameType, "Тип игры в созданном объекте неверный.");
        Assert.AreEqual(_players.Count, createdGame.Players.Count, "Количество игроков в созданном объекте неверное.");
        Assert.AreEqual(_players, createdGame.Players, "Список игроков в созданном объекте не совпадает с переданным.");
        Assert.AreEqual(1, createdGame.InstanceId, "ID первой созданной игры должен быть 1.");
    }

    /// <summary>
    /// Вспомогательный метод для получения доступа к приватному полю _activeMiniGames через рефлексию.
    /// </summary>
    private Dictionary<int, MiniGame> GetActiveGames(MiniGameService service)
    {
        var field = typeof(MiniGameService).GetField("_activeMiniGames", BindingFlags.NonPublic | BindingFlags.Instance);
        return field?.GetValue(service) as Dictionary<int, MiniGame>;
    }
}