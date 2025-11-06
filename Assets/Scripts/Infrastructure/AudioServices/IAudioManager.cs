namespace Infrastructure.AudioServices
{
    public interface IAudioManager
    {
        void SetMusicPlayer(AudioPlayer player);
        void PlayMusic();
        void StopMusic();
        IAudioData Data { get; }
    }
}