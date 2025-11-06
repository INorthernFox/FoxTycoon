using System;
using UniRx;
using UnityEngine;

namespace Infrastructure.InputSpace.Movers
{

    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private float _sensitivity = 10;
        
        private IInputManager _input;
        private IDisposable _movementSub;

        private Vector3 _moveVector;
        
        public void SetInputManager(IInputManager input)
        {
            _input = input;
            _movementSub = _input.Movement
                .Subscribe(OnMovementChanged)
                .AddTo(this);
        }

        private void OnMovementChanged(Vector2 move)
        {
            _moveVector = new Vector3(move.x, 0f, move.y) * Time.deltaTime * _sensitivity;
        }

        private void LateUpdate()
        {
            if (_moveVector == Vector3.zero)
                return;
            
            transform.position += _moveVector;
        }

        private void OnDestroy()
        {
            _movementSub?.Dispose();
            _input?.Dispose();
        }
    }
}