using System.Collections.Generic;
using CodeBase.Infrastructure.Data;
using FishNet.Object;

namespace CodeBase.Logic.Minigame
{
    public class MinigameManagerSinglton : NetworkBehaviour
    {
        public static MinigameManagerSinglton Instance { get; private set; }


        private Dictionary<int, QuestData> _questDataMap;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            // Initialize server-side logic here
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            // Initialize client-side logic here
        }
        
        
        [ServerRpc(RequireOwnership = false)]
        public void StartMinigame(int questId, int playerId)
        {
            // Logic to start the minigame

            if (_questDataMap.ContainsKey(questId))
            {
                if (_questDataMap[questId].CurrentPlayers.Contains(playerId))
                {
                    // Player already in the quest, no need to add again
                    return;
                }
                else
                {
                    _questDataMap[questId].CurrentPlayers.Add(playerId);
                }
            }
            else
            {
                _questDataMap[questId] = new QuestData
                {
                    Id = questId, Record = -1,
                };
                
                _questDataMap[questId].CurrentPlayers.Add(playerId);
            }
            
            
        }
        [ServerRpc(RequireOwnership = false)]
        public void EndMinigame(int questId, int playerId)
        {
            if (_questDataMap.ContainsKey(questId))
            {
                if (_questDataMap[questId].CurrentPlayers.Contains(playerId))
                {
                    _questDataMap[questId].CurrentPlayers.Remove(playerId);
                }
                
                // Optionally, check if the quest is complete and handle accordingly
                if (_questDataMap[questId].CurrentPlayers.Count == 0)
                {
                    _questDataMap.Remove(questId);
                }
            }
        }
    }
}