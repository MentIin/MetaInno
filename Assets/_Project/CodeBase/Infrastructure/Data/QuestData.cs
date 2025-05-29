using System.Collections.Generic;
using FishNet.Connection;

namespace CodeBase.Infrastructure.Data
{
    public class QuestData
    {
        public int Id;
        public float Record=-1;
        public List<NetworkConnection> CurrentPlayers = new List<NetworkConnection>();
    }
}