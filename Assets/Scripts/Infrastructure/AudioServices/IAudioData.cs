using UniRx;

namespace Infrastructure.AudioServices
{
    public interface IAudioData
    {
        IReadOnlyReactiveProperty<float> Volume { get; }
        void ChangeVolume(float volume);
    }
}