using UnityEngine;

namespace Infrastructure.InputSpace.Movers
{
    public class EdgePanCamera : CameraMover
    {
        [Header("Camera / Plane")]
        [SerializeField] private UnityEngine.Camera cam;
        [SerializeField] private Vector3 movePlaneNormal = Vector3.up;
        [SerializeField] private float maxSpeed = 15f;

        [Header("Edge trigger")]
        [SerializeField] private float enterThickness = 24f;
        [SerializeField] private float exitThickness = 42f;
        [SerializeField, Range(0.05f, 1f)] private float minSpeedFactorAtExit = 0.2f;

        [Header("Smoothing")]
        [SerializeField, Range(0f, 1f)] private float accelLerp = 0.2f; 

        private Vector3 _vel; 
        private bool _edgeActive; 
        private Vector2 _lastDir2D;

        void Update()
        {
            if(cam == null) 
                return;

            Vector3 mp = Input.mousePosition;
            float w = Screen.width;
            float h = Screen.height;

            if(Application.isEditor)
            {
                if(mp.x < 0 || mp.y < 0 || mp.x > w || mp.y > h)
                {
                    StopEdge();
                    ApplyMovement(Vector3.zero);
                    return;
                }
            }

            float distLeft = mp.x;
            float distRight = w - mp.x;
            float distBottom = mp.y;
            float distTop = h - mp.y;

            float minDist = Mathf.Min(distLeft, distRight, distBottom, distTop);

            if(!_edgeActive && minDist <= enterThickness)
                _edgeActive = true;
            else if(_edgeActive && minDist > exitThickness)
                _edgeActive = false;

            Vector2 dir2D;

            if(_edgeActive)
            {
                Vector2 instantDir = Vector2.zero;

                if(distLeft <= enterThickness) instantDir.x = -Remap01(distLeft, 0f, enterThickness);
                else if(distRight <= enterThickness) instantDir.x = Remap01(distRight, 0f, enterThickness);

                if(distBottom <= enterThickness) instantDir.y = -Remap01(distBottom, 0f, enterThickness);
                else if(distTop <= enterThickness) instantDir.y = Remap01(distTop, 0f, enterThickness);

                if(instantDir.sqrMagnitude > 1f) instantDir.Normalize();

                if(instantDir == Vector2.zero)
                {
                    dir2D = _lastDir2D; 
                }
                else
                {
                    dir2D = instantDir;
                    _lastDir2D = instantDir; 
                }
            }
            else
            {
                dir2D = Vector2.zero;
                _lastDir2D = Vector2.zero;
            }
            
            float edgeFactor = 0f;
            if(_edgeActive)
            {
                float t = 1f - Mathf.InverseLerp(exitThickness, enterThickness, minDist);
                edgeFactor = Mathf.Lerp(minSpeedFactorAtExit, 1f, Mathf.Clamp01(t));
            }

            Vector3 worldDir = ScreenDirToWorldXZ(dir2D);

            Vector3 targetVel = worldDir * (maxSpeed * edgeFactor);

            ApplyMovement(targetVel);
        }

        private void StopEdge()
        {
            _edgeActive = false;
            _lastDir2D = Vector2.zero;
        }

        private void ApplyMovement(Vector3 targetVel)
        {
            _vel = Vector3.Lerp(_vel, targetVel, 1f - accelLerp);

            if(_vel.sqrMagnitude > 0.0001f)
            {
                Vector3 newPos = transform.position + _vel * Time.deltaTime;
                transform.position = newPos;
            }
            else
            {
                _vel = Vector3.zero;
            }
        }

        private Vector3 ScreenDirToWorldXZ(Vector2 dir2D)
        {
            if(dir2D == Vector2.zero) 
                return Vector3.zero;

            Vector3 right = cam.transform.right;
            Vector3 fwd = Vector3.ProjectOnPlane(cam.transform.forward, movePlaneNormal).normalized;

            Vector3 world = right * dir2D.x + fwd * dir2D.y;
            world = Vector3.ProjectOnPlane(world, movePlaneNormal).normalized;
            return world;
        }

        private static float Remap01(float v, float a, float b)
        {
            if(Mathf.Approximately(a, b)) return 0f;
            return Mathf.Clamp01((v - a) / (b - a));
        }
    }
}