using System;
using CodeBase.Infrastructure.StaticData;
using CodeBase.UI.Services.Windows;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace CodeBase.Logic.Minigame
{
    [RequireComponent(typeof(SphereCollider))]
    public class ReceiveQuestPoint : NetworkBehaviour
    {
        public QuestStaticData QuestStaticData;

        private void OnTriggerEnter(Collider other)
        {
            if (IsClientInitialized)
            {
                if (other.CompareTag("Player"))
                {
                    NetworkObject player = other.GetComponent<NetworkObject>();
                    //ShowWindowClientRpc(player.Owner);
                    MinigameUISinglton.Instance.ShowQuestDialog(QuestStaticData);
                }
            }
        }

        [TargetRpc]
        private void ShowWindowClientRpc(NetworkConnection target)
        {
            MinigameUISinglton.Instance.ShowQuestDialog(QuestStaticData);
            MinigameManagerSinglton.Instance.LocalOwner = target;
        }
    }
}