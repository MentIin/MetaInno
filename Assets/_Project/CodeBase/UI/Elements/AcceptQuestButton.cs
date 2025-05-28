using CodeBase.Infrastructure.StaticData;
using CodeBase.Logic.Minigame;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements
{
    public class AcceptQuestButton : MonoBehaviour
    {
        public Button Button;

        private QuestStaticData _questStaticData;
        
        private void Awake()
        {
            if (Button == null)
                Button = GetComponent<Button>();

            Button.onClick.AddListener(AcceptQuest);
        }


        public void SetQuestData(QuestStaticData questStaticData)
        {
            _questStaticData = questStaticData;
        }

        private void AcceptQuest()
        {
            MinigameManagerSinglton.Instance.StartMinigame(_questStaticData);
            
            MinigameUISinglton.Instance.CloseWindow();
        }
    }
}