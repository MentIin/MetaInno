using System.Collections.Generic;
using CodeBase.Infrastructure.Services.MiniGameService;
using FishNet.Connection;
using NUnit.Framework;

// Required for Debug.Log in the original class, though usually mocked for tests

namespace CodeBase.Tests.Infrastructure.Services.MiniGame
{
    public class MiniGameServiceTests
    {
        private MiniGameService _miniGameService;

        [SetUp]
        public void SetUp()
        {
            _miniGameService = new MiniGameService();
        }

        [Test]
        public void CreateMiniGame_ShouldCreateNewMiniGameAndAddToActiveGames()
        {
            // Arrange
            MiniGameType gameType = MiniGameType.Race; // Assuming MiniGameType is an enum
            List<NetworkConnection> players = new List<NetworkConnection> { new NetworkConnection(), new NetworkConnection() };

            // Act
            CodeBase.Infrastructure.Services.MiniGameService.MiniGame newGame = _miniGameService.CreateMiniGame(gameType, players);

            // Assert
            Assert.NotNull(newGame, "The created mini-game should not be null.");
            Assert.Greater(newGame.InstanceId, 0, "The instance ID should be greater than 0.");
            Assert.AreEqual(gameType, newGame.GameType, "The game type should match the one provided.");
            Assert.AreEqual(players.Count, newGame.Players.Count, "The number of players should match the one provided.");
        }

        [Test]
        public void EndMiniGame_ShouldRemoveExistingMiniGameFromActiveGames()
        {
            // Arrange
            MiniGameType gameType = MiniGameType.Shooter;
            List<NetworkConnection> players = new List<NetworkConnection> { new NetworkConnection() };
            CodeBase.Infrastructure.Services.MiniGameService.MiniGame gameToCreateAndEnd = _miniGameService.CreateMiniGame(gameType, players);
            int instanceIdToEnd = gameToCreateAndEnd.InstanceId;

            // Act
            _miniGameService.EndMiniGame(instanceIdToEnd);
        }

        [Test]
        public void EndMiniGame_ShouldDoNothingWhenMiniGameDoesNotExist()
        {
            // Arrange
            int nonExistentInstanceId = 999; // An ID that surely doesn't exist
            _miniGameService.EndMiniGame(nonExistentInstanceId);
            Assert.Pass("Ending a non-existent mini-game should not throw an exception.");
        }
    }
}