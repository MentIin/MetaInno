using _Project.CodeBase.Infrastructure.Factory;
using _Project.CodeBase.Infrastructure.Services.StaticData;
using _Project.CodeBase.Logic.Camera.CameraLogic;
using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.Infrastructure.AssetManagement;
using Assets.CodeBase.Infrastructure.Services;
using Assets.CodeBase.Infrastructure.Services.AudioService;
using Assets.CodeBase.Infrastructure.Services.Input;
using Assets.CodeBase.Infrastructure.Services.Pause;
using Assets.CodeBase.Infrastructure.Services.PersistentProgress;
using Assets.CodeBase.Infrastructure.Services.Random;
using Assets.CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase._Project.CodeBase.Infrastructure.Services.Input;
using CodeBase._Project.CodeBase.Infrastructure.Services.StaticData;
using CodeBase._Project.CodeBase.UI.Services.UIFactory;
using CodeBase.Infrastructure.Services.AudioService;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _allServices;
        private readonly LoadingCurtain _loadingCurtain;
        private AudioSource _audioSource;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly CameraController _cameraController;
        
        public BootstrapState(IGameStateMachine gameStateMachine, SceneLoader sceneLoader, AllServices allServices,
            LoadingCurtain loadingCurtain, AudioSource audioSource, ICoroutineRunner coroutineRunner,
            CameraController cameraController, AudioMixerGroup musicMixerGroup, AudioMixerGroup soundsMixerGroup)
        {
            _audioSource = audioSource;
            _coroutineRunner = coroutineRunner;
            _cameraController = cameraController;
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _allServices = allServices;
            _loadingCurtain = loadingCurtain;
            RegisterServices(soundsMixerGroup, musicMixerGroup);
        }

        public void Exit()
        {
            
        }

        public void Enter()
        {
            //_gameStateMachine.Enter<MainMenuState>();
        }

        private void RegisterServices(AudioMixerGroup soundsMixerGroup, AudioMixerGroup musicMixerGroup)
        {
            
            
            _allServices.RegisterSingle<IRandomService>(new RandomService(662));
            
            
            _allServices.RegisterSingle<IAssets>(new Assets.CodeBase.Infrastructure.AssetManagement.Assets());
            _allServices.RegisterSingle<IStaticDataService>(new StaticDataService());
            _allServices.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            
            
            _allServices.RegisterSingle<IAudioService>(new AudioService(_audioSource,
                _allServices.Single<IAssets>(), _coroutineRunner,
                _allServices.Single<IPersistentProgressService>(), soundsMixerGroup,
                musicMixerGroup));
            _allServices.RegisterSingle<PauseService>(new PauseService(_allServices.Single<IAudioService>()));
            
            
            _allServices.RegisterSingle<IInputService>(new StandaloneInputService(_coroutineRunner));
            

            RegisterUIFactory();
            RegisterFactory();

            _allServices.Single<IInputService>().Initialize();
            
            
            _gameStateMachine.InitializeStates(this);
            _gameStateMachine.Enter<LoadProgressState>();
        }

        private void RegisterUIFactory()
        {
            _allServices.RegisterSingle<UIFactory>(new UIFactory(_allServices.Single<IAssets>(),
                _allServices.Single<IStaticDataService>(), _allServices.Single<IPersistentProgressService>(),
                _gameStateMachine, _allServices.Single<IAudioService>(), _allServices.Single<PauseService>(),
                _allServices.Single<ISaveLoadService>()));
        }

        private void RegisterFactory()
        {
            
        }
    }
}