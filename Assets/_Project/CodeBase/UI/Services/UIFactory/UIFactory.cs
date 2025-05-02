using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.CodeBase.Infrastructure.AssetManagement;
using Assets.CodeBase.Infrastructure.Services;
using Assets.CodeBase.Infrastructure.Services.Pause;
using Assets.CodeBase.Infrastructure.Services.PersistentProgress;
using Assets.CodeBase.Infrastructure.Services.SaveLoad;
using Assets.CodeBase.Infrastructure.Services.StaticData;
using Assets.CodeBase.StaticData;
using CodeBase._Project.CodeBase.Infrastructure.Services.StaticData;
using CodeBase._Project.CodeBase.UI.Services.Windows;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.AudioService;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase._Project.CodeBase.UI.Services.UIFactory
{
    class UIFactory : IService
    {
        private readonly IStaticDataService _staticData;
        private readonly IPersistentProgressService _persistentProgressService;
        private readonly IGameStateMachine _stateMachine;
        private readonly IAudioService _audioService;
        private readonly IAssets _assets;


        private Dictionary<WindowType, WindowBase> Windows = new Dictionary<WindowType, WindowBase>();

        public UIFactory(IAssets assets, IStaticDataService staticData,
            IPersistentProgressService persistentProgressService,
            IGameStateMachine stateMachine, IAudioService audioService,
            PauseService pauseService, ISaveLoadService saveLoadService)
        {
            _assets = assets;
            _staticData = staticData;
            _persistentProgressService = persistentProgressService;
            _stateMachine = stateMachine;
            _audioService = audioService;
        }

        public async Task CreateWindow(WindowType type)
        {
            
            WindowConfig data = _staticData.ForWindow(type);
            var prefab = data.WindowReference;
            WindowBase windowBase = Object.Instantiate(prefab.GetComponent<WindowBase>(), null);
            
            
        }

        

        public void Clear()
        {
            
        }

        public bool CheckWindowExistence(WindowType type)
        {
            if (Windows.ContainsKey(type))
            {
                if (Windows[type] != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}