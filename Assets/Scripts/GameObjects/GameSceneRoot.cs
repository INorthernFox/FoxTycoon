using GameObjects.Levels;
using UnityEngine;

namespace GameObjects
{
    public class GameSceneRoot : MonoBehaviour
    {
        private LevelObject _levelObject;

        public void SetLevelObject(LevelObject levelObject) =>
            _levelObject = levelObject;
    }
}