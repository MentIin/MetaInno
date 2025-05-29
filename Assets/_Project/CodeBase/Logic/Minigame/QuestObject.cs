using System;
using CodeBase.Infrastructure.StaticData;
using FishNet.Object;
using UnityEngine;

namespace CodeBase.Logic.Minigame
{
    public class QuestObject : NetworkBehaviour
    {
        public QuestStaticData QuestStaticData;

        public Mode Mode;
        
        public override void OnStartClient()
        {
            base.OnStartClient();
            MinigameManagerSinglton.Instance.QuestStarted += OnQuestStarted;
            MinigameManagerSinglton.Instance.QuestFinished += OnQuestFinished;
            
            
            
            if (Mode == Mode.ActivateWhenQuestStarted)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnQuestFinished(int id)
        {
            if (id != QuestStaticData.Id) return;
            
            
            if (Mode == Mode.ActivateWhenQuestStarted)
            {
                gameObject.SetActive(false);
            }else if (Mode == Mode.DeactivateWhenQuestStarted)
            {
                gameObject.SetActive(true);
            }
        }

        private void OnQuestStarted(int id)
        {
            Debug.Log(id);
            if (id != QuestStaticData.Id) return;
            
            
            if (Mode == Mode.ActivateWhenQuestStarted)
            {
                gameObject.SetActive(true);
            }else if (Mode == Mode.DeactivateWhenQuestStarted)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public enum Mode
    {
        None=0, DeactivateWhenQuestStarted=1, ActivateWhenQuestStarted=2
    }
}