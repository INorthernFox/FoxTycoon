using Constants;
using GameObjects.Levels;
using Infrastructure.Loggers;
using Infrastructure.Systems;
using UnityEngine.SceneManagement;

namespace GameObjects
{
    public class GameSceneInitializer
    {
        private readonly LevelsFactory _levelsFactory;
        private readonly LevelsDatabase _levelsDatabase;
        private readonly IGameLogger _logger;

        public GameSceneInitializer(
            LevelsFactory levelsFactory,
            LevelsDatabase levelsDatabase,
            IGameLogger logger)
        {
            _levelsFactory = levelsFactory;
            _levelsDatabase = levelsDatabase;
            _logger = logger;
        }

        public Scene InitializeGameScene(Scene scene)
        {
            GameSceneRoot root = scene.CreateRoot<GameSceneRoot>();

            //Пока нет выбора уровня
            if(_levelsDatabase.Settings == null || _levelsDatabase.Settings.Count == 0)
            {
                _logger.Log("LevelsDatabase.Settings is null or empty. Cannot initialize game scene.",
                    LogLevel.Error,
                    LogSystemType.Scene,
                    LogIds.GameSceneInitializer.LevelsDatabaseEmpty);
                return scene;
            }

            LevelSettings setting = _levelsDatabase.Settings[0];
            _levelsFactory.Create(root, setting);

            return scene;
        }
    }
}