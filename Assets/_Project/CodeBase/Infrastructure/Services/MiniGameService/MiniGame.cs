using System.Collections.Generic;
using FishNet.Connection;

namespace CodeBase.Infrastructure.Services.MiniGameService
{
    public enum MiniGameState
    {
        Lobby,
        InProgress,
        Finished
    }

    public enum MiniGameType
    {
        Race,
        Shooter,
        Puzzle
    }

    public class MiniGame
    {
        public int InstanceId { get; }
        public MiniGameType GameType { get; }
        public MiniGameState State { get; set; }
        public List<NetworkConnection> Players { get; }

        public MiniGame(int instanceId, MiniGameType gameType, List<NetworkConnection> initialPlayers)
        {
            InstanceId = instanceId;
            GameType = gameType;
            Players = initialPlayers;
            State = MiniGameState.Lobby;
        }
    }
}