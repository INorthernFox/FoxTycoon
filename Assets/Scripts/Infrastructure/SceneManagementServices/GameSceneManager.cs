using System;
using Constants;
using Infrastructure.Loggers;
using Infrastructure.SceneManagementServices.LoadingScreenService;
using Infrastructure.SystemExtensions;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.SceneManagementServices
{
    public sealed class GameSceneManager : ISceneManager
    {
        private readonly IGameLogger _logger;
        private readonly ILoadingScreenManager _loadingScreen;
        private readonly SceneSettings _settings;

        [Inject]
        public GameSceneManager(IGameLogger logger, ILoadingScreenManager loadingScreen, SceneSettings settings)
        {
            _logger = logger;
            _loadingScreen = loadingScreen;
            _settings = settings;
        }

        public IObservable<Scene> Load(SceneType type, LoadSceneMode mod = LoadSceneMode.Additive)
        {
            return Observable.Defer(() =>
                {
                    int id = _settings.BuildIndexOf(type);

                    var already = SceneManager.GetSceneByBuildIndex(id);
                    if(already.IsValid() && already.isLoaded)
                    {
                        _logger.Log($"Scene {type}  {id} ('{already.name}') already loaded.",
                            LogLevel.Info, LogSystemType.Scene, LogIds.GameSceneManager.LoadAlreadyLoaded);
                        return Observable.Return(already);
                    }

                    _logger.Log($"Loading scene ({mod}, async):  type={type}  buildIndex={id}",
                        LogLevel.Info, LogSystemType.Scene, LogIds.GameSceneManager.LoadStart);

                    _loadingScreen.TryShow();

                    AsyncOperation op = SceneManager.LoadSceneAsync(id, mod);

                    if(op == null)
                    {
                        _logger.Log($"LoadSceneAsync returned null for  type={type}  buildIndex={id}.",
                            LogLevel.Error, LogSystemType.Scene, LogIds.GameSceneManager.LoadOpNull);
                        return Observable.Throw<Scene>(new InvalidOperationException($"Cannot start loading scene {type} {id}"));
                    }

                    return op.ToObservable()
                        .Select(_ =>
                        {
                            var scene = SceneManager.GetSceneByBuildIndex(id);
                            if(!scene.IsValid() || !scene.isLoaded)
                            {
                                _logger.Log($"Scene {type} {id} not loaded after async operation.",
                                    LogLevel.Error, LogSystemType.Scene, LogIds.GameSceneManager.LoadNotLoaded);
                                throw new InvalidOperationException($"Scene {type} {id} not loaded after async operation");
                            }

                            _logger.Log($"Scene loaded:  type={type} buildIndex={id}, name='{scene.name}'.",
                                LogLevel.Info, LogSystemType.Scene, LogIds.GameSceneManager.LoadComplete);

                            _loadingScreen.TryHide();
                            return scene;
                        });
                })
                .ObserveOnMainThread();
        }

        public IObservable<SceneType> Unload(SceneType type)
        {
            return Observable.Defer(() =>
                {
                    int id = _settings.BuildIndexOf(type);

                    Scene scene = SceneManager.GetSceneByBuildIndex(id);

                    if(!scene.IsValid() || !scene.isLoaded)
                    {
                        _logger.Log($"Scene {id} is not loaded — nothing to unload.",
                            LogLevel.Warning, LogSystemType.Scene, LogIds.GameSceneManager.UnloadNoop);
                        return Observable.Return(type);
                    }

                    _logger.Log($"Unloading scene (async): type={type} buildIndex={id}, name='{scene.name}'.",
                        LogLevel.Info, LogSystemType.Scene, LogIds.GameSceneManager.UnloadStart);

                    AsyncOperation op = SceneManager.UnloadSceneAsync(id);

                    if(op == null)
                    {
                        _logger.Log($"UnloadSceneAsync returned null for type={type} buildIndex={id}.",
                            LogLevel.Error, LogSystemType.Scene, LogIds.GameSceneManager.UnloadOpNull);
                        return Observable.Throw<SceneType>(new InvalidOperationException($"Cannot start unloading scene {type} {id}"));
                    }

                    return op.ToObservable()
                        .Select(_ =>
                        {
                            _logger.Log($"Scene unloaded: buildIndex={id}.",
                                LogLevel.Info, LogSystemType.Scene, LogIds.GameSceneManager.UnloadComplete);

                            return type;
                        });
                })
                .ObserveOnMainThread();
        }
    }
}