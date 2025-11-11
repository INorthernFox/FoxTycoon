using UnityEngine;

namespace GameObjects.Players
{
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int Move = Animator.StringToHash("Move");
        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Collection = Animator.StringToHash("Collection");

        private readonly int[] _hashes = { Move, Idle, Collection };

        [SerializeField] private Animator _animator;

        private void Awake()
        {
            if(_animator == null)
                Debug.LogError($"Animator is not assigned on {gameObject.name}", this);
        }

        public void PlayIdle()
        {
            if(_animator == null)
                return;

            DisableAll();
            _animator.SetBool(Idle, true);
        }

        public void PlayMove()
        {
            if(_animator == null)
                return;

            DisableAll();
            _animator.SetBool(Move, true);
        }

        public void PlayCollection()
        {
            if(_animator == null)
                return;

            DisableAll();
            _animator.SetBool(Collection, true);
        }

        private void DisableAll()
        {
            if(_animator == null)
                return;

            foreach(int hash in _hashes)
                _animator.SetBool(hash, false);
        }
    }
}