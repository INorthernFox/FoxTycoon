using Infrastructure.GameSystemsInitializers;
using Infrastructure.SaveServices;
using Infrastructure.StateMachines.Games;
using Infrastructure.Systems;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SaveInstaller.Install(Container);
        SystemsInitializerInstaller.Install(Container);
        GameStateMachineInstaller.Install(Container);
        
        Container.Bind<SystemSceneInitializer>().AsSingle();
    }
}