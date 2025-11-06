using Infrastructure.AudioServices.Views;
using Zenject;

namespace Infrastructure.AudioServices.Systems
{
    public class AudioInstaller : MonoInstaller
    {
        public AudioPlayList AudioPlayList;
        public AudioPlayer PrefabPlayer;
        public AudioSettingsView SettingsViewPlayer;

        public override void InstallBindings()
        {
            Container.Bind<AudioPlayList>().FromInstance(AudioPlayList).AsSingle();
            Container.Bind<AudioPlayer>().FromInstance(PrefabPlayer).AsSingle();
            Container.Bind<AudioSettingsView>().FromInstance(SettingsViewPlayer).AsSingle();

            Container.Bind<AudioPlayerFactory>().AsSingle();
            Container.Bind<AudioSettingsViewFactory>().AsSingle();
            Container.Bind<AudioDataFactory>().AsSingle();
            Container.Bind<IAudioManager>().FromFactory<AudioManagerFactory>().AsSingle();
        }
    }
}