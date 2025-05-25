using System;
using CodeBase.Infrastructure.StaticData;
using CodeBase.UI.Services.Windows;
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
            if (!IsServer) return;
            
            if (other.CompareTag("Player"))
            {
                MinigameUISinglton.Instance.ShowQuestDialog(QuestStaticData);
                MinigameUISinglton.Instance.StartTimer(30f);
            }
        }
    }
}