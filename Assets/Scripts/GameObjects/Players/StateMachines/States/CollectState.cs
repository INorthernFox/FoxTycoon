using System;
using System.Collections.Generic;
using Infrastructure.StateMachines.Core;
using UniRx;

namespace GameObjects.Players.StateMachines.States
{
    public class CollectState : BaseState, IPlayerState
    {
        private readonly PlayerAnimator _animator;

        public CollectState(PlayerAnimator animator)
        {
            _animator = animator;
            ValidPredecessors = new HashSet<Type>
            {
                typeof(MoveState),
                typeof(IdleState)
            };
        }

        public override IObservable<IState> Enter()
        {
            _animator.PlayCollection();
            return Observable.Return(this);
        }
    }
}