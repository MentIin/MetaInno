using CodeBase.Infrastructure.StaticData;
using CodeBase.UI.Services.Windows;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace CodeBase.Logic.Minigame
{
    [RequireComponent(typeof(SphereCollider))]
    public class FinishQuestPoint : NetworkBehaviour
    {
        public QuestStaticData QuestStaticData;

        public ParticleSystem ParticleSystem;

        public void Start()
        {
            base.OnStartClient();
            ParticleSystem.transform.parent = null;
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (IsClientInitialized)
            {
                if (other.CompareTag("Player"))
                {
                    NetworkObject player = other.GetComponent<NetworkObject>();
                    //ShowWindowClientRpc(player.Owner);
                    //MinigameUISinglton.Instance.ShowQuestDialog
                    //(_currentQuestStaticData);
                    MinigameManagerSinglton.Instance.FinishMinigame(QuestStaticData, MinigameUISinglton.Instance.TimeLeft);
                    
                    ParticleSystem.Play();
                    MinigameUISinglton.Instance.PlayFinishAnimation();
                }
            }
        }
    }
}