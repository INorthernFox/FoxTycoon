using Infrastructure.AudioServices.Views;
using UnityEngine;
using Zenject;

namespace Infrastructure.AudioServices.Systems
{
    public class AudioSettingsViewFactory : IFactory<AudioRoot, AudioSettingsView>
    {
        private readonly IAudioManager _audioManager;
        private readonly AudioSettingsView _prefab;
        
        public AudioSettingsViewFactory(IAudioManager audioManager, AudioSettingsView prefab)
        {
            _audioManager = audioManager;
            _prefab = prefab;
        }

        public AudioSettingsView Create(AudioRoot param)
        {
            AudioSettingsView view = Object.Instantiate(_prefab, param.transform);
            view.Initialization(_audioManager);
            return view;
        }
    }
}