using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.Camera.CameraLogic;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.Audio;

namespace CodeBase.Infrastructure
{
    public class Game
    {
        public readonly IGameStateMachine StateMachine;

        public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain, AudioSource audioSource,
            CameraController cameraController, AudioMixerGroup soundsMixerGroup, AudioMixerGroup musicMixerGroup)
        {
            StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), coroutineRunner,
                AllServices.Container, loadingCurtain, audioSource, cameraController,
                musicMixerGroup, soundsMixerGroup);
        }
    }
}