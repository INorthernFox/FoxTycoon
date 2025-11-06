using GameObjects.Players.StateMachines.States;
using Infrastructure.Loggers;
using Infrastructure.StateMachines.Core;

namespace GameObjects.Players.StateMachines
{
    public class PlayerStateMachine : BaseStateMachine<IPlayerState>
    {
        public PlayerStateMachine(IGameLogger logger, IPlayerState[] states) : base(logger) =>
            RegisterState(states);
    }

}