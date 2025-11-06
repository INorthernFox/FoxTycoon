using Zenject;

namespace Infrastructure.AudioServices
{
    public class AudioManagerFactory : IFactory<AudioManager>
    {
        private readonly AudioDataFactory _dataFactory;

        public AudioManagerFactory(AudioDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        public AudioManager Create()
        {
            AudioData data = _dataFactory.Create();
            return new AudioManager(data);
        }
    }
}