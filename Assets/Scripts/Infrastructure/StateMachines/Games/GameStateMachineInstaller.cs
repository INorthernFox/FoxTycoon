using Infrastructure.StateMachines.Games.States;
using Infrastructure.StateMachines.Games.States.Interfaces;
using Zenject;

namespace Infrastructure.StateMachines.Games
{
    public class GameStateMachineInstaller : Installer<GameStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            BindGameStates();
            Container.Bind<GameStateMachine>().AsSingle();
        }
        
        private void BindGameStates()
        {
            Container.Bind<IGameState>().To<BootstrapState>().AsSingle();
            Container.Bind<IGameState>().To<LoadField>().AsSingle();
            Container.Bind<IGameState>().To<Game>().AsSingle();
        }
    }
}