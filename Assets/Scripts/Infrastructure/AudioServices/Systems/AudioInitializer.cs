using Infrastructure.AudioServices.Views;
using Infrastructure.Systems;
using Zenject;

namespace Infrastructure.AudioServices.Systems
{
    public static class AudioInitializer
    {
        public static SystemSceneRoots AddAudio(this SystemSceneRoots systemSceneRoot, DiContainer container)
        {
            AudioRoot root = Extensions.Create<AudioRoot>(systemSceneRoot.transform);
            
            AudioPlayerFactory playerFactory = container.Resolve<AudioPlayerFactory>();
            AudioPlayer visual = playerFactory.Create(root);
            
            AudioSettingsViewFactory viewFactory = container.Resolve<AudioSettingsViewFactory>();
            AudioSettingsView settings = viewFactory.Create(root);
            
            return systemSceneRoot;
        }
    }

}