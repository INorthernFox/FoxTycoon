using System;
using System.Collections.Generic;
using Constants;
using Infrastructure.Loggers;
using UniRx;

namespace Infrastructure.StateMachines.Core
{
    public abstract class BaseStateMachine<TStateType> : IStateMachine, IDisposable
        where TStateType : IState
    {
        private readonly Dictionary<Type, IState> _states = new();
        private readonly ReactiveProperty<IState> _currentState = new();
        private IDisposable _subscription;
        private readonly IGameLogger _logger;

        public IReadOnlyReactiveProperty<IState> Current => _currentState;

        protected BaseStateMachine(IGameLogger logger)
        {
            _logger = logger;
        }

        public void RegisterState<T>(T[] states) where T : TStateType
        {
            foreach(T state in states)
                _states[state.GetType()] = state;
        }

        public void RegisterState<T>(T state) where T : IState =>
            _states[state.GetType()] = state;

        public IObservable<Type> ChangeStateTo<TState>() 
            where TState : IState
        {
            if (!TryGetState(out TState next))
                return Observable.Empty<Type>();

            return ChangeStateCore(() => next.Enter());
        }

        public IObservable<Type> ChangeStateTo<TState, TValue>(TValue value) 
            where TState : IState<TValue>
        {
            if (!TryGetState(out TState next))
                return Observable.Empty<Type>();

            return ChangeStateCore(() => next.Enter(value));
        }
      
        private bool TryGetState<TState>(out TState nextState) where TState : IState
        {
            nextState = default( TState );
            Type type = typeof(TState);
            Type currentType = _currentState.Value?.GetType();

            if(!_states.TryGetValue(type, out IState state) || state is not TState tState)
            {
                LogNotRegisteredState(type);
                return false;
            }

            if(currentType != null && !state.ValidatePredecessors(currentType))
            {
                LogInvalidStateTransition(type, currentType);
                return false;
            }

            if(state == _currentState.Value)
            {
                LogTransitionSameState(type);
                return false;
            }

            nextState = tState;
            return true;
        }


        private IObservable<Type> ChangeStateCore(Func<IObservable<IState>> enterInvoker)
        {
            IObservable<Type> pipeline = Observable.Defer(() =>
            {
                IObservable<IState> exit = _currentState.Value != null
                    ? _currentState.Value.Exit()
                    : Observable.Return<IState>(null);

                return exit
                    .SelectMany(_ => enterInvoker())
                    .Do(nextState => _currentState.Value = nextState)
                    .Select(nextState => nextState?.GetType())
                    .Take(1);
            });

            IObservable<Type> shared = pipeline.Replay(1).RefCount();

            _subscription?.Dispose();

            _subscription = shared.Subscribe();

            return shared;
        }

        private void LogInvalidStateTransition(Type targetType, Type currentType) =>
            _logger.Log(
                $"Invalid state transition: cannot change from '{currentType.Name}' to '{targetType.Name}'.",
                LogLevel.Warning,
                LogSystemType.Core,
                LogIds.StateMachines.InvalidStateTransition);

        private void LogTransitionSameState(Type type) =>
            _logger.Log(
                $"Attempted to transition to the same state '{type.Name}'. Transition ignored.",
                LogLevel.Warning,
                LogSystemType.Core,
                LogIds.StateMachines.TransitionSameState);

        private void LogNotRegisteredState(Type type) =>
            _logger.Log(
                $"Attempted to transition to state '{type.Name}', but it is not registered in the state machine.",
                LogLevel.Error,
                LogSystemType.Core,
                LogIds.StateMachines.NotRegisteredState);

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}