using GameObjects.Buildings;
using GameObjects.Players;
using Zenject;

namespace GameObjects.Levels
{
    public class LevelInstaller : MonoInstaller
    {
        public LevelsDatabase LevelsDatabase;
        public BuildingObject BuildingObject;
        public PlayerObject PlayerObject;
        public PlayerResourcesView PlayerResourcesView;
        
        public override void InstallBindings()
        {
            Container.Bind<LevelsDatabase>().FromInstance(LevelsDatabase).AsSingle();
            Container.Bind<BuildingObject>().FromInstance(BuildingObject).AsSingle();
            Container.Bind<PlayerObject>().FromInstance(PlayerObject).AsSingle();
            Container.Bind<PlayerResourcesView>().FromInstance(PlayerResourcesView).AsSingle();
            Container.Bind<BuildingFactory>().AsSingle();
            Container.Bind<LevelsFactory>().AsSingle();
            Container.Bind<PlayerFactory>().AsSingle();
            Container.Bind<GameSceneInitializer>().AsSingle();
        }
    }
}