using CodeBase.Infrastructure.StaticData;
using CodeBase.UI.Elements;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.UI.Services.Windows
{
    public class MinigameUISinglton : MonoBehaviour
    {
        [SerializeField] private Animator _questDialogAnimator;
        
        [SerializeField] private TextMeshProUGUI _questTitleText;
        [SerializeField] private TextMeshProUGUI _timeForQuestText;
        [SerializeField] private TextMeshProUGUI _timerText;

        [SerializeField] private AcceptQuestButton _acceptQuestButton;
        
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

            _timerText.text = "";
        }
        
        
        public void ShowQuestDialog(QuestStaticData questStaticData)
        {
            _questDialogAnimator.SetBool("active", true);
            _questTitleText.text = questStaticData.Title;
            //QuestDescriptionText.text = questStaticData.Description;
            _currentQuestStaticData = questStaticData;
            _timeForQuestText.text = questStaticData.Time.ToString() + "c.";
            
            _acceptQuestButton.SetQuestData(questStaticData);
        }
        
        
        public void CloseWindow()
        {
            _questDialogAnimator.SetBool("active", false);
        }
        
        
        public void StartTimer(float duration)
        {
            _timerText.text = duration.ToString();
        }
        public void StopTimer()
        {
            _timerText.text = "";
        }

        
    }
}