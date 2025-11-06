using System;
using UniRx;
using UnityEngine;

namespace Infrastructure.InputSpace.Raycasters
{
    public class GroundRaycaster : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private LayerMask _groundMask = ~0;
        [SerializeField] private float _maxDistance = 1000f;

        private CompositeDisposable _compositeDisposable =  new();

        public IObservable<GroundRayHit> RayHits => _rayHits;
        private readonly Subject<GroundRayHit> _rayHits = new();
    
        private IInputManager _inputManager;

        public void SetInputManager(IInputManager inputManager)
        {
            _inputManager = inputManager;

            _inputManager.Clicked.Subscribe(HandleTap).AddTo(_compositeDisposable);
        }
    
        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
            _compositeDisposable = null;
        }

        private void HandleTap(Vector2 screenPos)
        {
            Ray ray = _camera.ScreenPointToRay(screenPos);
        
            if(!Physics.Raycast(ray, out RaycastHit hitInfo, _maxDistance, _groundMask, QueryTriggerInteraction.Ignore))
                return;
        
            Debug.Log(hitInfo.point);
        
            GroundRayHit hit = new()
            {
                Hit = true,
                Point = hitInfo.point,
                Normal = hitInfo.normal,
                Collider = hitInfo.collider,
                Tag = hitInfo.collider != null ? hitInfo.collider.tag : string.Empty
            };
            
            _rayHits.OnNext(hit);
        }
    }
}