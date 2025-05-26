using CodeBase.Infrastructure.StaticData;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Services.Windows
{
    public class MinigameUISinglton : MonoBehaviour
    {
        [SerializeField] private GameObject QuestDialogContainer;
        [SerializeField] private TextMeshPro QuestTitleText;
        [SerializeField] private TextMeshPro QuestDescriptionText;
        
        
        [SerializeField] private TextMeshPro TimerText;
        
        private QuestStaticData _currentQuestStaticData;
        
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
        
        
        public void ShowQuestDialog(QuestStaticData questStaticData)
        {
            QuestDialogContainer.SetActive(true);
            QuestTitleText.text = questStaticData.Title;
            QuestDescriptionText.text = questStaticData.Description;
            _currentQuestStaticData = questStaticData;
            
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