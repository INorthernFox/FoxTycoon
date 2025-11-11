using System;
using UniRx;
using UnityEngine;

namespace Infrastructure.InputSpace.Movers
{
    public class CameraMover : MonoBehaviour
    {
        [Header("Settings Profile")]
        [SerializeField] private CameraMoverSettings _settingsProfile;
        [SerializeField] private bool _useProfile = true;

        [Header("Speed Settings (если не используется профиль)")]
        [SerializeField] private AnimationCurve _moveSpeed;
        [SerializeField] private float _baseSpeed = 10f;
        [SerializeField] private float _maxSpeed = 20f;

        [Header("Smoothing (для плавности на mobile)")]
        [SerializeField] private float _smoothTime = 0.15f;
        [SerializeField] private float _dampening = 5f;

        [Header("Inertia (инерция после отпускания)")]
        [SerializeField] private bool _useInertia = true;
        [SerializeField] private float _inertiaDecay = 0.9f;

        [Header("Camera Bounds (границы камеры)")]
        [SerializeField] private bool _useBounds = true;
        [SerializeField] private Vector2 _minBounds = new Vector2(-50f, -50f);
        [SerializeField] private Vector2 _maxBounds = new Vector2(50f, 50f);

        [Header("Edge Slowdown (замедление у краев)")]
        [SerializeField] private bool _useEdgeSlowdown = true;
        [SerializeField] private float _edgeSlowdownDistance = 5f;
        
        private float BaseSpeed => _useProfile && _settingsProfile != null ? _settingsProfile.BaseSpeed : _baseSpeed;
        private float MaxSpeed => _useProfile && _settingsProfile != null ? _settingsProfile.MaxSpeed : _maxSpeed;
        private float SmoothTime => _useProfile && _settingsProfile != null ? _settingsProfile.SmoothTime : _smoothTime;
        private float Dampening => _useProfile && _settingsProfile != null ? _settingsProfile.Dampening : _dampening;
        private bool UseInertia => _useProfile && _settingsProfile != null ? _settingsProfile.UseInertia : _useInertia;
        private float InertiaDecay => _useProfile && _settingsProfile != null ? _settingsProfile.InertiaDecay : _inertiaDecay;
        private bool UseBounds => _useProfile && _settingsProfile != null ? _settingsProfile.UseBounds : _useBounds;
        private Vector2 MinBounds => _useProfile && _settingsProfile != null ? _settingsProfile.MinBounds : _minBounds;
        private Vector2 MaxBounds => _useProfile && _settingsProfile != null ? _settingsProfile.MaxBounds : _maxBounds;
        private bool UseEdgeSlowdown => _useProfile && _settingsProfile != null ? _settingsProfile.UseEdgeSlowdown : _useEdgeSlowdown;
        private float EdgeSlowdownDistance => _useProfile && _settingsProfile != null ? _settingsProfile.EdgeSlowdownDistance : _edgeSlowdownDistance;

        private float _time = 0;
        private IInputManager _input;
        private IDisposable _movementSub;

        private Vector3 _targetMoveVector;
        private Vector3 _currentMoveVector;
        private Vector3 _currentVelocity;
        private Vector3 _inertiaVelocity;

        public void SetInputManager(IInputManager input)
        {
            _input = input;
            _movementSub = _input.Movement
                .Subscribe(OnMovementChanged)
                .AddTo(this);
        }

        private void OnMovementChanged(Vector2 move) =>
            _targetMoveVector = new Vector3(move.x, 0f, move.y);

        private void LateUpdate()
        {
            bool isInputActive = _targetMoveVector != Vector3.zero;

            if (isInputActive)
            {
                _time += Time.deltaTime;

                _currentMoveVector = Vector3.SmoothDamp(
                    _currentMoveVector,
                    _targetMoveVector,
                    ref _currentVelocity,
                    SmoothTime
                );

                float speedMultiplier = _moveSpeed?.Evaluate(_time) ?? 1f;
                Vector3 movement = _currentMoveVector * Time.deltaTime * BaseSpeed * speedMultiplier;

                if (UseInertia)
                    _inertiaVelocity = movement / Time.deltaTime;

                ApplyMovement(movement);
            }
            else
            {
                _time = 0;

                if (UseInertia && _inertiaVelocity.magnitude > 0.01f)
                {
                    _inertiaVelocity *= InertiaDecay;
                    ApplyMovement(_inertiaVelocity * Time.deltaTime);
                }
                else
                {
                    _currentMoveVector = Vector3.Lerp(_currentMoveVector, Vector3.zero, Time.deltaTime * Dampening);
                    _inertiaVelocity = Vector3.zero;
                }
            }
        }

        private void ApplyMovement(Vector3 movement)
        {
            Vector3 newPosition = transform.position + movement;

            if (UseEdgeSlowdown && UseBounds)
            {
                float edgeFactor = CalculateEdgeSlowdownFactor(newPosition);
                movement *= edgeFactor;
                newPosition = transform.position + movement;
            }

            if (UseBounds)
            {
                newPosition.x = Mathf.Clamp(newPosition.x, MinBounds.x, MaxBounds.x);
                newPosition.z = Mathf.Clamp(newPosition.z, MinBounds.y, MaxBounds.y);
            }

            Vector3 clampedMovement = Vector3.ClampMagnitude(newPosition - transform.position, MaxSpeed * Time.deltaTime);
            transform.position += clampedMovement;
        }

        private float CalculateEdgeSlowdownFactor(Vector3 position)
        {
            float factorX = 1f;
            float factorZ = 1f;

            Vector2 minBounds = MinBounds;
            Vector2 maxBounds = MaxBounds;
            float slowdownDist = EdgeSlowdownDistance;

            float distToMinX = position.x - minBounds.x;
            float distToMaxX = maxBounds.x - position.x;
            if (distToMinX < slowdownDist)
                factorX = Mathf.Clamp01(distToMinX / slowdownDist);
            else if (distToMaxX < slowdownDist)
                factorX = Mathf.Clamp01(distToMaxX / slowdownDist);

            float distToMinZ = position.z - minBounds.y;
            float distToMaxZ = maxBounds.y - position.z;
            if (distToMinZ < slowdownDist)
                factorZ = Mathf.Clamp01(distToMinZ / slowdownDist);
            else if (distToMaxZ < slowdownDist)
                factorZ = Mathf.Clamp01(distToMaxZ / slowdownDist);

            return Mathf.Min(factorX, factorZ);
        }

        private void OnDestroy()
        {
            _movementSub?.Dispose();
        }

        private void OnDrawGizmosSelected()
        {
            if (!UseBounds) return;

            Vector2 minBounds = MinBounds;
            Vector2 maxBounds = MaxBounds;

            Gizmos.color = Color.yellow;
            Vector3 bottomLeft = new(minBounds.x, transform.position.y, minBounds.y);
            Vector3 bottomRight = new(maxBounds.x, transform.position.y, minBounds.y);
            Vector3 topLeft = new(minBounds.x, transform.position.y, maxBounds.y);
            Vector3 topRight = new(maxBounds.x, transform.position.y, maxBounds.y);

            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);
            Gizmos.DrawLine(topRight, topLeft);
            Gizmos.DrawLine(topLeft, bottomLeft);

            if (UseEdgeSlowdown)
            {
                float slowdownDist = EdgeSlowdownDistance;
                Gizmos.color = Color.red * 0.5f;
                Vector3 innerBL = new(minBounds.x + slowdownDist, transform.position.y, minBounds.y + slowdownDist);
                Vector3 innerBR = new(maxBounds.x - slowdownDist, transform.position.y, minBounds.y + slowdownDist);
                Vector3 innerTL = new(minBounds.x + slowdownDist, transform.position.y, maxBounds.y - slowdownDist);
                Vector3 innerTR = new(maxBounds.x - slowdownDist, transform.position.y, maxBounds.y - slowdownDist);

                Gizmos.DrawLine(innerBL, innerBR);
                Gizmos.DrawLine(innerBR, innerTR);
                Gizmos.DrawLine(innerTR, innerTL);
                Gizmos.DrawLine(innerTL, innerBL);
            }
        }
    }
}