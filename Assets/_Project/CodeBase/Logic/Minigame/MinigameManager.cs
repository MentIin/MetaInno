using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Data;
using CodeBase.Infrastructure.StaticData;
using CodeBase.UI.Services.Windows;
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
        public event Action<int> QuestFinished;
        public event Action<int> QuestFail;

        public event Action<int, float> QuestRecordUpdated;


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
            
        }

        [ServerRpc(RequireOwnership = false)]
        public void RequestRecordsUpdateServerRPC()
        {
            foreach (var pair in _questDataMap)
            {
                UpdateRecordObserverRPC(pair.Key, pair.Value.Record);
            }
        }

        public void StartMinigame(QuestStaticData questStaticData)
        {
            Debug.Log(1);
            if (LocalOwner == null)
            {
                LocalOwner = InstanceFinder.ClientManager.Connection;
            }
            
            Debug.Log(LocalOwner);
            
            StartMinigameServerRPC(questStaticData.Id, LocalOwner, questStaticData.Time);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void StartMinigameServerRPC(int questId, NetworkConnection playerId, float dur)
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
            StartMinigameClientRPC(playerId, questId, dur);
        }

        [TargetRpc(Logging = LoggingType.Common)]
        private void StartMinigameClientRPC(NetworkConnection networkConnection, int questId, float duration)
        {
            Debug.Log(3);
            QuestStarted?.Invoke(questId);
            
            MinigameUISinglton.Instance.StartTimer(duration, networkConnection, questId);
        }
        
        
        
        public void FinishMinigame(QuestStaticData questStaticData, float timeLeft)
        {
            FinishMinigameServerRPC(questStaticData.Id, LocalOwner, timeLeft);
        }
        
        [ServerRpc(RequireOwnership = false)]
        public void FinishMinigameServerRPC(int questId, NetworkConnection playerConnection, float timeLeft)
        {
            if (_questDataMap.ContainsKey(questId))
            {
                if (_questDataMap[questId].CurrentPlayers.Contains(playerConnection))
                {
                    _questDataMap[questId].CurrentPlayers.Remove(playerConnection);
                }


                if (_questDataMap[questId].Record < timeLeft || _questDataMap[questId].Record == -1)
                {
                    _questDataMap[questId].Record = timeLeft;
                }
                
                FinishMinigameClientRPC(playerConnection, questId);

                UpdateRecordObserverRPC(questId, _questDataMap[questId].Record);
            }
        }

        [ObserversRpc]
        private void UpdateRecordObserverRPC(int id, float record)
        {
            QuestRecordUpdated?.Invoke(id, record);
        }

        [TargetRpc(Logging = LoggingType.Common)]
        private void FinishMinigameClientRPC(NetworkConnection networkConnection, int questId)
        {
            QuestFinished?.Invoke(questId);
            
            MinigameUISinglton.Instance.StopTimer();
        }

        

        public void FailMinigame(NetworkConnection networkConnection, int questId)
        {
            
            FailMinigameServerRPC(networkConnection, questId);
        }

        private void FailMinigameServerRPC(NetworkConnection playerConnection, int questId)
        {
            if (_questDataMap.ContainsKey(questId))
            {
                if (_questDataMap[questId].CurrentPlayers.Contains(playerConnection))
                {
                    _questDataMap[questId].CurrentPlayers.Remove(playerConnection);
                }
                

                FailMinigameTargetRPC(playerConnection, questId);
            }
        }

        [TargetRpc]
        private void FailMinigameTargetRPC(NetworkConnection playerConnection, int questId)
        {
            QuestFail?.Invoke(questId);
        }
    }
}