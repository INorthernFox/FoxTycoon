using System;
using System.Collections.Generic;
using UniRx;

namespace Infrastructure.StateMachines.Core
{
    public abstract class BaseState : IState, IDisposable
    {
        protected HashSet<Type> ValidPredecessors;

        protected readonly Subject<IState> OnEnter = new();
        protected readonly Subject<IState> OnExit = new();

        protected BaseState()
        {
            ValidPredecessors = new HashSet<Type>();
        }

        public virtual IObservable<IState> Enter() =>
            Observable.Return(this);
        public virtual IObservable<IState> Exit()=>
            Observable.Return(this);

        public bool ValidatePredecessors(Type type) =>
            ValidPredecessors?.Contains(type) ?? false;

        public virtual void Dispose()
        {
            OnEnter?.Dispose();
            OnExit?.Dispose();
        }
    }
}