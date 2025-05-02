using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.Infrastructure.Services.GetUserInfoService;
using Assets.CodeBase.Infrastructure.Services.Pause;
using Assets.CodeBase.Infrastructure.Services.PersistentProgress;
using Assets.CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase._Project.CodeBase.Infrastructure.Data;
using CodeBase._Project.CodeBase.Infrastructure.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private GameStateMachine _stateMachine;
        private IPersistentProgressService _progressService;
        private ISaveLoadService _saveLoadService;
        private readonly IStaticDataService _staticDataService;


        private Coroutine _analyticServiceCoroutine;
        private bool _analyticServiceTimeout=false;

        public LoadProgressState(GameStateMachine stateMachine,
            IPersistentProgressService progressService,
            ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _progressService = progressService;
            _saveLoadService = saveLoadService;

        }

        public void Enter()
        {
            _staticDataService.Load();
            if (_saveLoadService.LoadProgress() == null) _progressService.Progress = GetNewProgress();
            else _progressService.Progress = _saveLoadService.LoadProgress();
        }
        
        public void Exit()
        {
            
        }
        private void LoadProgressOrInitNew()
        {
            _progressService.Progress =  _saveLoadService.LoadProgress() ?? GetNewProgress();
        }

        private PlayerProgress GetNewProgress()
        {
            PlayerProgress playerProgress = new PlayerProgress();
            
            return playerProgress;
        }
    }
}