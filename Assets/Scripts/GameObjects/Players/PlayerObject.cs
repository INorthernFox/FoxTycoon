using System;
using GameObjects.ResourcesSpace;
using GameObjects.ResourcesSpace.Views;
using UniRx;
using UnityEngine;

namespace GameObjects.Players
{
    public class PlayerObject : MonoBehaviour
    {
        [SerializeField] private PlayerMover _mover;
        [SerializeField] private ResourcesTransferView _resourcesTransferView;
        [SerializeField] private PlayerAnimator _animator;

        private readonly CompositeDisposable _disposables = new();
        
        public PlayerAnimator Animator => _animator;
        public ResourcesTransferView ResourcesTransferView => _resourcesTransferView;
        public PlayerMover Mover => _mover;

        public Player Model { get; private set; }

        public void SetModel(Player player) =>
            Model = player;
        
        public void AddToObjectStream(IDisposable stream) =>
            _disposables.Add(stream);

        private void OnDestroy() =>
            _disposables.Dispose();
    }

}