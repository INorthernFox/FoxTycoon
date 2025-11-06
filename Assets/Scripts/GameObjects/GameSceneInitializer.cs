using GameObjects.Levels;
using Infrastructure.Systems;
using UnityEngine.SceneManagement;

namespace GameObjects
{
    public class GameSceneInitializer
    {
        private readonly LevelsFactory _levelsFactory;
        private readonly LevelsDatabase _levelsDatabase;

        public GameSceneInitializer(
            LevelsFactory levelsFactory,
            LevelsDatabase levelsDatabase)
        {
            _levelsFactory = levelsFactory;
            _levelsDatabase = levelsDatabase;
        }

        public Scene InitializeGameScene(Scene scene)
        {
            GameSceneRoot root = scene.CreateRoot<GameSceneRoot>();

            //Пока нет выбора уровня
            LevelSettings setting = _levelsDatabase.Settings[0];
            _levelsFactory.Create(root, setting);

            return scene;
        }
    }
}