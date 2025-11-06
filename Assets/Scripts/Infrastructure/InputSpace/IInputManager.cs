using System;
using UniRx;
using UnityEngine;

namespace Infrastructure.InputSpace
{
    public interface IInputManager : IDisposable
    {
        IReadOnlyReactiveProperty<Vector2> Movement { get; }
        IObservable<Vector2> Clicked { get; }
    }
}