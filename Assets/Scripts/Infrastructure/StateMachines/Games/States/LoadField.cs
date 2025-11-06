using System;
using System.Collections.Generic;
using GameObjects;
using Infrastructure.SceneManagementServices;
using Infrastructure.StateMachines.Core;
using Infrastructure.StateMachines.Games.States.Interfaces;
using UniRx;
using Zenject;

namespace Infrastructure.StateMachines.Games.States
{
    public class LoadField : BaseState, IGameState
    {
        private readonly ISceneManager _sceneManager;
        private readonly GameSceneInitializer _initializer;

        [Inject]
        public LoadField(
            ISceneManager sceneManager,
            GameSceneInitializer initializer)
        {
            _sceneManager = sceneManager;
            _initializer = initializer;

            ValidPredecessors = new HashSet<Type>
            {
                typeof(BootstrapState),
            };
        }

        public override IObservable<IState> Enter()
        {
            return _sceneManager.Load(SceneType.Game).Take(1)
                .Do(scene => _initializer.InitializeGameScene(scene))
                .ObserveOnMainThread()
                .Do(_ => OnEnter.OnNext(this))
                .Select(_ => this)
                .Take(1);
        }

        public override IObservable<IState> Exit() =>
            Observable.Return(this);
    }
}