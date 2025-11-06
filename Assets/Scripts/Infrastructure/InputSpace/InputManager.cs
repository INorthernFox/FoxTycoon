using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.InputSpace
{

    public class InputManager : IInputManager
    {
        private readonly MainInput _mainInput;

        public IReadOnlyReactiveProperty<Vector2> Movement => _movement;
        public IObservable<Vector2> Clicked => _click;

        private readonly ReactiveProperty<Vector2> _movement = new(Vector2.zero);
        private readonly Subject<Vector2> _click = new();

        public InputManager()
        {
            _mainInput = new MainInput();

            _mainInput.Player.CameraMovement.performed += OnMovePerformed;
            _mainInput.Player.CameraMovement.canceled += OnMoveCanceled;

            _mainInput.Player.Click.performed += OnClickPerformed;

            _mainInput.Enable();
        }

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            _click.OnNext(screenPos);
        }

        private void OnMovePerformed(InputAction.CallbackContext context) =>
            _movement.Value = context.ReadValue<Vector2>();

        private void OnMoveCanceled(InputAction.CallbackContext context) =>
            _movement.Value = Vector2.zero;

        public void Dispose()
        {
            _mainInput.Player.CameraMovement.performed -= OnMovePerformed;
            _mainInput.Player.CameraMovement.canceled -= OnMoveCanceled;

            _mainInput.Disable();

            _movement?.Dispose();
            _click?.Dispose();
        }
    }

}