using System;
using UnityEngine.SceneManagement;

namespace Infrastructure.SceneManagementServices
{
    public interface ISceneManager
    {
        public IObservable<Scene> Load(SceneType type,  LoadSceneMode mod = LoadSceneMode.Additive);
        public IObservable<SceneType> Unload(SceneType type);
    }

}