using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.AudioServices
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _player;

        private AudioPlayList _playList;
        private readonly CompositeDisposable _streams = new();

        private void Awake()
        {
            if (_player == null)
            {
                Debug.LogError($"AudioSource is not assigned on {gameObject.name}", this);
            }
        }

        public void SetPlaylist(AudioPlayList playList) =>
            _playList = playList;

        public void SetData(IAudioData data)
        {
            if (_player == null) return;

            data.Volume.Subscribe(x => _player.volume = x).AddTo(_streams);
        }

        public void Play()
        {
            if (_player == null || _playList == null || _playList.Clips == null || _playList.Clips.Length == 0)
                return;

            int number = Random.Range(0, _playList.Clips.Length);
            _player.clip = _playList.Clips[number];
            _player.Play();
        }

        public void Stop()
        {
            if (_player == null) return;

            _player.Stop();
        }

        private void OnDestroy() =>
            _streams.Dispose();

    }

}