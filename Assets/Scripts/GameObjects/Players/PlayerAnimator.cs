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

        public void PlayIdle()
        {
            DisableAll();
            _animator.SetBool(Idle, true);
        }

        public void PlayMove()
        {
            DisableAll();
            _animator.SetBool(Move, true);
        }

        public void PlayCollection()
        {
            DisableAll();
            _animator.SetBool(Collection, true);
        }

        private void DisableAll()
        {
            foreach(var hash in _hashes)
                _animator.SetBool(hash, false);
        }
    }
}