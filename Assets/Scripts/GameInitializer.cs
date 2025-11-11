using Infrastructure.StateMachines.Games;
using Infrastructure.StateMachines.Games.States;
using UniRx;
using UnityEngine;
using Zenject;

public class GameInitializer : MonoBehaviour
{
    [Inject] private GameStateMachine _stateMachine;

    private readonly CompositeDisposable _disposables = new();

    private void Awake()
    {
        _stateMachine.ChangeStateTo<BootstrapState>()
            .Where(newState => newState == typeof(BootstrapState))
            .Do(_ => _stateMachine.ChangeStateTo<LoadField>())
            .Take(1)
            .Subscribe()
            .AddTo(_disposables);
    }

    private void OnDestroy() => _disposables.Dispose();
}