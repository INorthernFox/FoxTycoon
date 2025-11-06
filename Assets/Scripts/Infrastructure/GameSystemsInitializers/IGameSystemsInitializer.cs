using System;
using UniRx;

namespace Infrastructure.GameSystemsInitializers
{
    public interface IGameSystemsInitializer
    {
        IObservable<Unit> InitSystems();
        IObservable<Unit> PreloadSystems();
    }

}