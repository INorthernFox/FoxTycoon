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

        public void SetPlaylist(AudioPlayList playList) =>
            _playList = playList;

        public void SetData(IAudioData data) =>
            data.Volume.Subscribe(x => _player.volume = x).AddTo(_streams);

        public void Play()
        {
            int number = Random.Range(0, _playList.Clips.Length);
            _player.clip = _playList.Clips[number];
            _player.Play();
        }

        public void Stop() =>
            _player.Stop();

        private void OnDestroy() =>
            _streams.Dispose();

    }

}