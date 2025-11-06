using UnityEngine;
using Zenject;

namespace Infrastructure.AudioServices.Systems
{
    public class AudioPlayerFactory : IFactory<AudioRoot, AudioPlayer>
    {
        private readonly IAudioManager _audioManager;
        private readonly AudioPlayList _playList;
        private readonly AudioPlayer _prefab;

        public AudioPlayerFactory(
            IAudioManager audioManager,
            AudioPlayList playList, 
            AudioPlayer prefab)
        {
            _audioManager = audioManager;
            _playList = playList;
            _prefab = prefab;
        }

        public AudioPlayer Create(AudioRoot param)
        {
            AudioPlayer player = Object.Instantiate(_prefab, param.transform);
            player.SetPlaylist(_playList);
            player.SetData(_audioManager.Data);
            _audioManager.SetMusicPlayer(player);
            return player;
        }
    }

}