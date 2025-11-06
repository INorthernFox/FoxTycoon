using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.Systems
{
    public static class Extensions
    {
        public static T Create<T>(Transform parent)
            where T : SystemRoot
        {
            GameObject gameObject = new();
            gameObject.transform.SetParent(parent);
            T component = gameObject.AddComponent<T>();
            gameObject.name = component.GetType().Name;
            return component;
        }
        
        public static T CreateRoot<T>(this Scene scene) 
            where T : Component
        {
            GameObject gameObject = new();
            T systemSceneRoot = gameObject.AddComponent<T>();
            gameObject.name = systemSceneRoot.GetType().Name;
            SceneManager.MoveGameObjectToScene(gameObject, scene);
            return systemSceneRoot;
        }
    }
}