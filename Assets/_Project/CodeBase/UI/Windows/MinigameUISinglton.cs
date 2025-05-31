using System;
using System.Collections;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.StaticData;
using CodeBase.Logic.Minigame;
using CodeBase.UI.Elements;
using DG.Tweening;
using FishNet.Connection;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CodeBase.UI.Services.Windows
{
    public class MinigameUISinglton : MonoBehaviour
    {
        [FormerlySerializedAs("_questDialogAnimator")] [SerializeField] private Animator _animator;
        
        
        [SerializeField] private TextMeshProUGUI _questTitleText;
        [SerializeField] private TextMeshProUGUI _timeForQuestText;
        
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private GameObject _timerContainer;

        [SerializeField] private AcceptQuestButton _acceptQuestButton;

        [Space(10)]
        [SerializeField] private RectTransform _finishPanel;
        private QuestStaticData _currentQuestStaticData;
        
        private static MinigameUISinglton _instance;


        private float _timerLeft;
        public float TimeLeft => _timerLeft;

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

            StopTimer();
        }
        


        public void ShowQuestDialog(QuestStaticData questStaticData)
        {
            _animator.SetBool("active", true);
            _questTitleText.text = questStaticData.Title;
            //QuestDescriptionText.text = questStaticData.Description;
            _currentQuestStaticData = questStaticData;
            _timeForQuestText.text = questStaticData.Time.ToString() + "c.";
            
            _acceptQuestButton.SetQuestData(questStaticData);
        }
        
        
        public void CloseWindow()
        {
            _animator.SetBool("active", false);
        }
        
        
        
        public void StartTimer(float duration, NetworkConnection networkConnection, int questId)
        {
            _timerContainer.SetActive(true);
            StartCoroutine(Timer(duration, networkConnection, questId));
        }

        private IEnumerator Timer(float duration, NetworkConnection networkConnection, int questId)
        {
            _timerLeft = duration;
            while (true)
            {
                _timerText.text = Mathf.RoundToInt(_timerLeft).ToString();
                _timerLeft -= Time.deltaTime;

                if (_timerLeft < 0)
                {
                    MinigameManagerSinglton.Instance.FailMinigame(networkConnection, questId);
                    StopTimer();
                    break;
                }
                yield return null;
            }
            
            
            
            
        }
        public void StopTimer()
        {
            _timerText.text = "";
            _timerContainer.SetActive(false);
            StopAllCoroutines();
        }

        public void PlayFinishAnimation()
        {
            DOTween.Sequence().Append(_finishPanel.DOScale(Vector3.one, 1f).SetEase(Ease.OutElastic)).
                Append(_finishPanel.DOScale(Vector3.one, 1f))
                .Append(_finishPanel.DOScale(Vector3.one * 0.01f, .3f))
                .Append(_finishPanel.DOScale(Vector3.one * 0f, .1f));


        }
    }
}