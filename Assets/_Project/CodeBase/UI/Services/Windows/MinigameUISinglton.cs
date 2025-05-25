using CodeBase.Infrastructure.StaticData;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Services.Windows
{
    public class MinigameUISinglton : MonoBehaviour
    {
        [SerializeField] private GameObject QuestDialogContainer;
        [SerializeField] private TextMeshPro TimerText;
        
        private static MinigameUISinglton _instance;
        public static MinigameUISinglton Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MinigameUISinglton>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("MinigameUISinglton");
                        _instance = singletonObject.AddComponent<MinigameUISinglton>();
                    }
                }
                return _instance;
            }
        }
        
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        
        // Add methods and properties to manage the Minigame UI here.
        public void ShowQuestDialog(QuestStaticData questStaticData)
        {
            QuestDialogContainer.SetActive(true);
        }
        public void HideQuestDialog()
        {
            QuestDialogContainer.SetActive(false);
        }
        public void StartTimer(float duration)
        {
            TimerText.text = duration.ToString();
        }
        public void StopTimer()
        {
            TimerText.text = "";
        }
        
        
    }
}