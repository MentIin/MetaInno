using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Data;
using CodeBase.Infrastructure.StaticData;
using FishNet;
using FishNet.Connection;
using FishNet.Managing.Logging;
using FishNet.Object;
using UnityEngine;

namespace CodeBase.Logic.Minigame
{
    public class MinigameManagerSinglton : NetworkBehaviour
    {
        public static MinigameManagerSinglton Instance { get; private set; }
        
        [HideInInspector]
        public NetworkConnection LocalOwner;
        
        private Dictionary<int, QuestData> _questDataMap = new Dictionary<int, QuestData>();


        public event Action<int> QuestStarted;


        public override void OnStartServer()
        {
            base.OnStartServer();
            // Initialize server-side logic here
        }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (Instance == null)
            {
                Instance = this;
            }
            
            // Initialize client-side logic here
        }
        public void StartMinigame(QuestStaticData questStaticData)
        {
            Debug.Log(1);
            if (LocalOwner == null)
            {
                LocalOwner = InstanceFinder.ClientManager.Connection;
            }
            
            Debug.Log(LocalOwner);
            
            StartMinigameServerRPC(questStaticData.Id, LocalOwner);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void StartMinigameServerRPC(int questId, NetworkConnection playerId)
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
            
            // TODO targetRpc
            Debug.Log(222222);
            Debug.Log(playerId);
            StartMinigameClientRPC(playerId, questId);
        }

        [TargetRpc(Logging = LoggingType.Common)]
        private void StartMinigameClientRPC(NetworkConnection networkConnection, int questId)
        {
            Debug.Log(3);
            QuestStarted?.Invoke(questId);
        }
        
        
        
        [ServerRpc(RequireOwnership = false)]
        public void EndMinigame(int questId, NetworkConnection playerConnection)
        {
            if (_questDataMap.ContainsKey(questId))
            {
                if (_questDataMap[questId].CurrentPlayers.Contains(playerConnection))
                {
                    _questDataMap[questId].CurrentPlayers.Remove(playerConnection);
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