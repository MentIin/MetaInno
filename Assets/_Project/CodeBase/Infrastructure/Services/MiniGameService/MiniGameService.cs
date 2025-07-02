using System.Collections.Generic;
using FishNet.Connection;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.MiniGameService
{

    /// <summary>
    /// Сервис для управления жизненным циклом мини-игр на сервере.
    /// </summary>
    public class MiniGameService : IService
    {
        private readonly Dictionary<int, MiniGame> _activeMiniGames = new Dictionary<int, MiniGame>();
        private int _nextInstanceId = 1;

        public MiniGame CreateMiniGame(MiniGameType gameType, List<NetworkConnection> players)
        {
            int instanceId = _nextInstanceId++;

            var newGame = new MiniGame(instanceId, gameType, players);
            _activeMiniGames.Add(instanceId, newGame);

            Debug.Log($"[MiniGameService] Created mini-game {gameType} with ID {instanceId} for {players.Count} players.");

            return newGame;
        }
        public void EndMiniGame(int instanceId)
        {
            if (!_activeMiniGames.TryGetValue(instanceId, out var gameToEnd))
            {
                Debug.LogWarning($"[MiniGameService] Trying to end a non-existent mini-game with ID {instanceId}.");
                return;
            }

            Debug.Log($"[MiniGameService] Ending mini-game {instanceId}");

            _activeMiniGames.Remove(instanceId);
        }
    }
}
