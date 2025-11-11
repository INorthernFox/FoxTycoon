using UnityEngine;

namespace Infrastructure.InputSpace.Movers
{
    [CreateAssetMenu(menuName = "Game/Camera/Mover Settings", fileName = "CameraMoverSettings", order = 0)]
    public class CameraMoverSettings : ScriptableObject
    {
        [Header("Speed Settings")]
        [Tooltip("Базовая скорость движения камеры")]
        public float BaseSpeed = 10f;

        [Tooltip("Максимальная скорость (для предотвращения рывков)")]
        public float MaxSpeed = 20f;

        [Header("Smoothing (для плавности на mobile)")]
        [Tooltip("Время сглаживания (меньше = быстрее реакция, больше = плавнее)")]
        [Range(0.05f, 0.5f)]
        public float SmoothTime = 0.15f;

        [Tooltip("Скорость затухания при остановке")]
        [Range(1f, 10f)]
        public float Dampening = 5f;

        [Header("Inertia (инерция после отпускания)")]
        [Tooltip("Использовать инерцию после отпускания пальца/мыши")]
        public bool UseInertia = true;

        [Tooltip("Скорость затухания инерции (0.9 = медленно, 0.5 = быстро)")]
        [Range(0.5f, 0.99f)]
        public float InertiaDecay = 0.9f;

        [Header("Camera Bounds (границы камеры)")]
        [Tooltip("Ограничить движение камеры границами")]
        public bool UseBounds = true;
        public Vector2 MinBounds = new Vector2(-50f, -50f);
        public Vector2 MaxBounds = new Vector2(50f, 50f);

        [Header("Edge Slowdown (замедление у краев)")]
        [Tooltip("Замедлять камеру при приближении к краям")]
        public bool UseEdgeSlowdown = true;

        [Tooltip("Расстояние от края, на котором начинается замедление")]
        [Range(1f, 10f)]
        public float EdgeSlowdownDistance = 5f;
    }
}
