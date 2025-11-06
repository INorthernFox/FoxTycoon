using System;
using UniRx;

namespace Infrastructure.StateMachines.Core
{
    public interface IStateMachine
    {
        public void RegisterState<TState>(TState state) 
            where TState : IState;
        public IObservable<Type> ChangeStateTo<TState>() 
            where TState : IState;
        public IObservable<Type> ChangeStateTo<TState,TValue>(TValue value) 
            where TState : IState<TValue>;
        IReadOnlyReactiveProperty<IState> Current { get; }
    }

}