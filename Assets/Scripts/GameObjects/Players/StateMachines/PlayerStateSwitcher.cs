using System;
using GameObjects.Players.StateMachines.States;
using UniRx;

namespace GameObjects.Players.StateMachines
{
    public class PlayerStateSwitcher : IDisposable
    {
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly PlayerMover _playerMover;
        private readonly Player _player;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        public PlayerStateSwitcher(PlayerStateMachine playerStateMachine, PlayerMover playerMover, Player player)
        {
            _playerStateMachine = playerStateMachine;
            _playerMover = playerMover;
            _player = player;
        }

        public void Initialization()
        {
            _playerStateMachine.ChangeStateTo<IdleState>();

            _playerMover.IsMoving
                .Where(x => x)
                .Subscribe(_ => _playerStateMachine.ChangeStateTo<MoveState>())
                .AddTo(_compositeDisposable);
            
            _playerMover.IsMoving
                .Where(x => !x && _player.IsCollected.Value)
                .Subscribe(_ => _playerStateMachine.ChangeStateTo<CollectState>())
                .AddTo(_compositeDisposable);
            
            _playerMover.IsMoving
                .Where(x => !x && !_player.IsCollected.Value)
                .Subscribe(_ => _playerStateMachine.ChangeStateTo<IdleState>())
                .AddTo(_compositeDisposable);
        }
        
        public void Dispose() =>
            _compositeDisposable.Dispose();
    }
}