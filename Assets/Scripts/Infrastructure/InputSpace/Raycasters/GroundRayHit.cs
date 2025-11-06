using System;
using UnityEngine;

namespace Infrastructure.InputSpace.Raycasters
{
    [Serializable]
    public struct GroundRayHit
    {
        public bool Hit;
        public Vector3 Point;
        public Vector3 Normal;
        public Collider Collider;
        public string Tag;
    }
}