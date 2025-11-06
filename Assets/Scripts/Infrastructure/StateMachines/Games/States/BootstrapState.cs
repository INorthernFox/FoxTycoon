using System;
using System.Collections.Generic;
using Infrastructure.AudioServices;
using Infrastructure.GameSystemsInitializers;
using Infrastructure.SceneManagementServices;
using Infrastructure.StateMachines.Core;
using Infrastructure.StateMachines.Games.States.Interfaces;
using Infrastructure.Systems;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.StateMachines.Games.States
{

    public class BootstrapState : BaseState, IGameState
    {
        private readonly ISceneManager _sceneManager;
        private readonly IGameSystemsInitializer _systemsInitializer;
        private readonly IAudioManager _audioManager;
        private readonly SystemSceneInitializer _systemSceneInitializer;

        [Inject]
        public BootstrapState(
            ISceneManager sceneManager,
            IGameSystemsInitializer systemsInitializer,
            IAudioManager audioManager,
            SystemSceneInitializer systemSceneInitializer)
        {
            _sceneManager = sceneManager;
            _systemsInitializer = systemsInitializer;
            _audioManager = audioManager;
            _systemSceneInitializer = systemSceneInitializer;

            ValidPredecessors = new HashSet<Type>();
        }

        public override IObservable<IState> Enter()
        {
            return _sceneManager.Load(SceneType.Bootstrap, LoadSceneMode.Single).Take(1)
                .SelectMany(_ => _systemsInitializer.PreloadSystems())
                .SelectMany(_ => _sceneManager.Load(SceneType.System).Take(1))
                .Do(scene => _systemSceneInitializer.AddSystemComponents(scene))
                .SelectMany(_ => _systemsInitializer.InitSystems())
                .ObserveOnMainThread()
                .Do(_ => OnEnter.OnNext(this))
                .Select(_ => this)
                .Take(1);
        }

        public override IObservable<IState> Exit()
        {
            return _sceneManager
                .Unload(SceneType.Bootstrap)
                .Do(_ => _audioManager.PlayMusic())
                .Do(_ => OnExit.OnNext(this))
                .Select(_ => this)
                .Take(1);
        }
    }

}