using System;
using GameObjects.Buildings;
using UniRx;
using UnityEngine;

namespace Infrastructure.InputSpace.Raycasters
{
    public class RayHitHandler : IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new();
    
        public IObservable<Vector3> MoveRequest => _moveRequest;
        private readonly Subject<Vector3> _moveRequest = new();
    
        public void SubscribeToRayHits(IObservable<GroundRayHit> observer)
        {
            observer
                .Subscribe(Handle)
                .AddTo(_compositeDisposable);
        }
    
        private void Handle(GroundRayHit groundRayHit)
        {
            Vector3 position = groundRayHit.Point;
        
            if(groundRayHit.Collider.transform.TryGetComponent(out BuildingObject buildingObject))
                position = buildingObject.CollectTrigger.Target.position;
        
            _moveRequest.OnNext(position);
        }
    
        public void Dispose() =>
            _compositeDisposable.Dispose();
    }
}