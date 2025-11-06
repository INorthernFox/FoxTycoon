using System;
using System.Collections.Generic;
using Infrastructure.SaveServices;
using Infrastructure.StateMachines.Core;
using Infrastructure.StateMachines.Games.States.Interfaces;
using UniRx;

namespace Infrastructure.StateMachines.Games.States
{
    public class Game : BaseState, IGameState
    {
        private readonly AutoSaver _autoSaver;
        
        public Game(AutoSaver  autoSaver)
        {
            _autoSaver = autoSaver;
            ValidPredecessors = new HashSet<Type> { typeof(LoadField) };
        }

        public override IObservable<IState> Enter()
        {
            _autoSaver.Start();
            
            return Observable.Return(this);
        }

        public override IObservable<IState> Exit()
        {
            _autoSaver.Stop();
            
            return Observable.Return(this);
        }
    }
}