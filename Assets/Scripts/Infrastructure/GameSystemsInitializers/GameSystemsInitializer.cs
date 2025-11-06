using System;
using Infrastructure.SaveServices.Interfaces;
using UniRx;

namespace Infrastructure.GameSystemsInitializers
{
    public class GameSystemsInitializer : IGameSystemsInitializer
    {
        private readonly ISaveManager _saveManager;
        
        public GameSystemsInitializer(ISaveManager saveManager)
        {
            _saveManager = saveManager;
        }

        public IObservable<Unit> PreloadSystems()
        {
            _saveManager.Load();
            
            return Observable.Return(Unit.Default);
        }
        
        public IObservable<Unit> InitSystems()
        {
            
            return Observable.Return(Unit.Default);
        }

    }

}