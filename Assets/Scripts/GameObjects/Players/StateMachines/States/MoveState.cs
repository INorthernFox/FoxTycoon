using System;
using System.Collections.Generic;
using Infrastructure.StateMachines.Core;
using UniRx;

namespace GameObjects.Players.StateMachines.States
{
    public class MoveState : BaseState, IPlayerState
    {
        private readonly PlayerAnimator _animator;

        public MoveState(PlayerAnimator animator)
        {
            _animator = animator;
            ValidPredecessors = new HashSet<Type>
            {
                typeof(CollectState),
                typeof(IdleState)
            };
        }

        public override IObservable<IState> Enter()
        {
            _animator.PlayMove();
            return Observable.Return(this);
        }
    }
}