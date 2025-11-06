using System;
using UniRx;
using UnityEngine;

namespace Infrastructure.SystemExtensions
{
    public  static class AsyncOperationRxExtensions
    {
        public static IObservable<AsyncOperation> ToObservable(this AsyncOperation op)
        {
            return Observable.Create<AsyncOperation>(observer =>
                {
                    if (op == null)
                    {
                        observer.OnError(new ArgumentNullException(nameof(op)));
                        return Disposable.Empty;
                    }

                    if (op.isDone)
                    {
                        observer.OnNext(op);
                        observer.OnCompleted();
                        return Disposable.Empty;
                    }

                    void OnCompleted(AsyncOperation _)
                    {
                        observer.OnNext(op);
                        observer.OnCompleted();
                    }

                    op.completed += OnCompleted;

                    return Disposable.Create(() => op.completed -= OnCompleted);
                    
                })
                .ObserveOnMainThread(); 
        }
    }
}