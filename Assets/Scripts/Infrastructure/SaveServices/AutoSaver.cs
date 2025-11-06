using System;
using Constants;
using Infrastructure.Loggers;
using Infrastructure.SaveServices.Interfaces;
using UniRx;

namespace Infrastructure.SaveServices
{
    public class AutoSaver : IDisposable
    {
        private readonly ISaveManager _saveManager;
        private readonly IGameLogger _logger;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(10);
        private readonly bool _saveOnStart = false;

        private IDisposable _subscription;
        private readonly CompositeDisposable _disposable = new();
        private bool _running;

        public AutoSaver(
            ISaveManager saveManager, 
            IGameLogger  logger)
        {
            _saveManager = saveManager ?? throw new ArgumentNullException(nameof(saveManager));
            _logger = logger;
        }

        public void Start()
        {
            if (_running) 
                return;
            
            _running = true;

            if (_saveOnStart)
                SafeSave();

            _subscription = Observable.Interval(_period)
                .ObserveOnMainThread()
                .Subscribe(_ => SafeSave())
                .AddTo(_disposable);
            
        }

        public void Stop()
        {
            if (!_running) return;
            _running = false;

            _subscription?.Dispose();
            _subscription = null;
        }
        
        private void SafeSave()
        {
            try
            {
                _saveManager.SaveAll();
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, LogLevel.Error, LogSystemType.Save, LogIds.Save.AutoSaveSafeSave);
            }
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}