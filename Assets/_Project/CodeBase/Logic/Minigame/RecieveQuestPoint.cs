using System;
using CodeBase.Infrastructure.StaticData;
using CodeBase.UI.Services.Windows;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CodeBase.Logic.Minigame
{
    [RequireComponent(typeof(SphereCollider))]
    public class ReceiveQuestPoint : NetworkBehaviour
    {
        public QuestStaticData[] Quests;
        private QuestStaticData _currentQuestStaticData;

        private void OnTriggerEnter(Collider other)
        {
            if (IsClientInitialized)
            {
                if (other.CompareTag("Player"))
                {
                    NetworkObject player = other.GetComponent<NetworkObject>();
                    //ShowWindowClientRpc(player.Owner);
                    if (player.IsOwner)
                    {
                        _currentQuestStaticData = Quests[Random.Range(0, Quests.Length)];
                          
                        MinigameUISinglton.Instance.ShowQuestDialog(_currentQuestStaticData);
                    }
                }
            }
        }

        [TargetRpc]
        private void ShowWindowClientRpc(NetworkConnection target)
        {
            MinigameUISinglton.Instance.ShowQuestDialog(_currentQuestStaticData);
            MinigameManagerSinglton.Instance.LocalOwner = target;
        }
    }
}