using Infrastructure.Loggers;
using Infrastructure.StateMachines.Core;
using Infrastructure.StateMachines.Games.States.Interfaces;
using Zenject;

namespace Infrastructure.StateMachines.Games
{
    public class GameStateMachine : BaseStateMachine<IGameState>
    {
        [Inject]
        public GameStateMachine(IGameLogger logger, IGameState[] states) : base(logger)
        {
            RegisterState(states);
        }
    }

}