using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace GameObjects.Players
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        private readonly CompositeDisposable _compositeDisposable = new();

        public IReadOnlyReactiveProperty<bool> IsMoving => _isMoving;
        private readonly ReactiveProperty<bool> _isMoving = new();

        public void SubscribeToMoveRequest(IObservable<Vector3> observable) =>
            observable.Subscribe(MoveTo).AddTo(_compositeDisposable);

        private void MoveTo(Vector3 position)
        {
            _navMeshAgent.SetDestination(position);
            _isMoving.Value = true;
        }

        private void Update()
        {
            if(_navMeshAgent.pathPending) 
                return;

            if(!_isMoving.Value)
                return;

            if(_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
                return;

            if(_navMeshAgent.hasPath && _navMeshAgent.velocity.sqrMagnitude >= 0.01f)
                return;

            _isMoving.Value = false;
            _navMeshAgent.Warp(transform.position);
        }

        private void OnDestroy() =>
            _compositeDisposable.Dispose();
    }
}