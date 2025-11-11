using Infrastructure.InputSpace.Movers;
using Infrastructure.InputSpace.Raycasters;
using Infrastructure.Loggers;
using UnityEngine;

namespace Infrastructure.InputSpace.Camera
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private CameraMover _cameraMover;
        [SerializeField] private GroundRaycaster _groundRaycaster;
        private IGameLogger _logger;

        public GroundRaycaster GroundRaycaster => _groundRaycaster;

        public void SetLogger(IGameLogger logger) =>
            _logger = logger;

        public void ValidateObject()
        {
            if(_cameraMover == null)
                _logger.Log($"[MainCamera] CameraMover is not assigned on {gameObject.name}", LogLevel.Error, LogSystemType.Camera);
            if(_groundRaycaster == null)
                _logger.Log($"[MainCamera] GroundRaycaster is not assigned on {gameObject.name}", LogLevel.Error, LogSystemType.Camera);
        }

        public void SetInputManager(IInputManager inputManager)
        {
            _cameraMover?.SetInputManager(inputManager);
            _groundRaycaster?.SetInputManager(inputManager);
        }
    }
}