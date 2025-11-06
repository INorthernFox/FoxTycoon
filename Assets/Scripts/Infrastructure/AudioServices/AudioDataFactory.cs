using Infrastructure.SaveServices.Interfaces;
using Zenject;

namespace Infrastructure.AudioServices
{
    public class AudioDataFactory : IFactory<AudioData>
    {
        private readonly ISaveManager _saveManager;
        
        public AudioDataFactory(ISaveManager saveManager) =>
            _saveManager = saveManager;

        public AudioData Create()
        {
            AudioData data = new AudioData();
            _saveManager.RegisterSaveable(data);
            return data;
        }
    }
}