using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic.Camera.CameraLogic;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Infrastructure.States
{
    public class GameStateMachine : BaseStateMachine, IGameStateMachine
    {
        private readonly SceneLoader _sceneLoader;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly AllServices _allServices;
        private readonly LoadingCurtain _loadingCurtain;
        private readonly AudioSource _audioSource;
        private readonly CameraController _cameraController;
        private readonly AudioMixerGroup _musicMixerGroup;
        private readonly AudioMixerGroup _soundsMixerGroup;

        public GameStateMachine(SceneLoader sceneLoader, ICoroutineRunner coroutineRunner, AllServices allServices,
            LoadingCurtain loadingCurtain, AudioSource audioSource, CameraController cameraController,
            AudioMixerGroup musicMixerGroup, AudioMixerGroup soundsMixerGroup)
        {
            _sceneLoader = sceneLoader;
            _coroutineRunner = coroutineRunner;
            _allServices = allServices;
            _loadingCurtain = loadingCurtain;
            _audioSource = audioSource;
            _cameraController = cameraController;
            _musicMixerGroup = musicMixerGroup;
            _soundsMixerGroup = soundsMixerGroup;
            
            BootstrapState bootstrapState = new BootstrapState(this, sceneLoader, allServices, loadingCurtain, audioSource,
                coroutineRunner, cameraController, musicMixerGroup, soundsMixerGroup);

                _states = new Dictionary<Type, IExitableState>()
                {
                    [typeof(BootstrapState)] = bootstrapState
                };
            }

        public void InitializeStates(BootstrapState bootstrapState)
        {
            _states[typeof(BootstrapState)] = bootstrapState;
            _states[typeof(GameLoopState)] = new GameLoopState(this);

            _states[typeof(LoadLevelState)] = new LoadLevelState(this);
            

            _states[typeof(LoadProgressState)] = new LoadProgressState(this,
                _allServices.Single<IPersistentProgressService>(),
                _allServices.Single<ISaveLoadService>());
            
                
        }
    }
}