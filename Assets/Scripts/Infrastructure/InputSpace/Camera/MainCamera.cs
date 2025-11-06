using Infrastructure.InputSpace.Movers;
using Infrastructure.InputSpace.Raycasters;
using UnityEngine;

namespace Infrastructure.InputSpace.Camera
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private CameraMover  _cameraMover;
        [SerializeField] private GroundRaycaster  _groundRaycaster;

        public GroundRaycaster  GroundRaycaster => _groundRaycaster;
        
        public void SetInputManager(IInputManager inputManager)
        {
            _cameraMover.SetInputManager(inputManager);
            _groundRaycaster.SetInputManager(inputManager);
        }
    }
}