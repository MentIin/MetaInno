using Assets.CodeBase.Infrastructure;
using Assets.CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.Services.AudioService;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    class GameLoopState : IState
    {


        public GameLoopState(IGameStateMachine gameStateMachine)
        {
            
        }

        public void Exit()
        {
            
        }

        public void Enter()
        {
            Debug.Log("Enter GameLoop");
        }
    }
}