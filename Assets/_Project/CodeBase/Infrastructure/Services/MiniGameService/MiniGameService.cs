using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Scened;
// Для доступа к InstanceFinder.ServerManager

// Для управления сценами


namespace CodeBase.Infrastructure.Services.MiniGameService
{
    public class MiniGameService : IService
    {
        private readonly Dictionary<int, MiniGame> _activeMiniGames = new Dictionary<int, MiniGame>();
        private int _nextInstanceId = 1;
        
        public MiniGame CreateMiniGame(MiniGameType gameType, List<NetworkConnection> players)
        {
            int instanceId = _nextInstanceId++;

            // 1. Создаем объект игры и регистрируем его
            var newGame = new MiniGame(instanceId, gameType, players);
            _activeMiniGames.Add(instanceId, newGame);
        
            UnityEngine.Debug.Log($"[MiniGameService] Created mini-game {gameType} with ID {instanceId} for {players.Count} players.");

            return newGame;
        }
        
        public void EndMiniGame(int instanceId, string hubSceneName = "HubWorld") // Укажите имя вашей главной сцены
        {
            if (!_activeMiniGames.TryGetValue(instanceId, out var gameToEnd))
            {
                UnityEngine.Debug.LogWarning($"[MiniGameService] Trying to end a non-existent mini-game with ID {instanceId}.");
                return;
            }

            UnityEngine.Debug.Log($"[MiniGameService] Ending mini-game {instanceId}. Returning players to {hubSceneName}.");
        
            // 2. Удаляем игру из списка активных
            _activeMiniGames.Remove(instanceId);
        }
    }
}