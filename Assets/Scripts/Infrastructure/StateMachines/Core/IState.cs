using System;

namespace Infrastructure.StateMachines.Core
{
    public interface IState
    {
        IObservable<IState> Enter();
        IObservable<IState> Exit();

        bool ValidatePredecessors(Type type);
    }

    public interface IState<TInput> : IState
    {
        IObservable<IState> Enter(in TInput value);
    }
}