using System;
using System.Collections.Generic;
using UniRx;

namespace Infrastructure.StateMachines.Core
{
    public abstract class BaseState : IState
    {
        protected HashSet<Type> ValidPredecessors;

        protected readonly Subject<IState> OnEnter = new();
        protected  readonly Subject<IState> OnExit = new();

        public virtual IObservable<IState> Enter() => 
            Observable.Return(this);
        public virtual IObservable<IState> Exit()=> 
            Observable.Return(this);

        public bool ValidatePredecessors(Type type) =>
            ValidPredecessors.Contains(type);
    }
}