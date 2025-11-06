namespace Infrastructure.AudioServices
{
    public class AudioManager : IAudioManager
    {
        private readonly AudioData _data;
        private AudioPlayer _musicPlayer;
        
        public IAudioData Data => _data;

        public AudioManager(AudioData data)
        {
            _data = data;
        }

        public void SetMusicPlayer(AudioPlayer player)
        {
            _musicPlayer = player;
        }

        public void PlayMusic() =>
            _musicPlayer.Play();
        
        public void StopMusic() =>
            _musicPlayer.Stop();
    }


}