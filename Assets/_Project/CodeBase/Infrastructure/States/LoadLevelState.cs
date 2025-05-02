namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;


        public LoadLevelState(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}