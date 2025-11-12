using System;
using GameObjects.Players;
using GameObjects.ResourcesSpace;
using UniRx;

namespace GameObjects.Buildings
{
    public class Building : IDisposable
    {
        private readonly BuildingData _data;
        private readonly ResourcesSettings _settings;
        private readonly CompositeDisposable _disposables = new();
        private float _fractionAccumulator;
        private float _transferAccumulator;
        private bool _isCollecting;
        private float _collectSeconds;
        private Player _collector;

        private const float TickInterval = 0.25f;
        
        public Building(BuildingData data, ResourcesSettings settings)
        {
            _data = data;
            _settings = settings;
        }

        public void StartProduction()
        {
            _disposables.Clear();
            Observable.Interval(TimeSpan.FromSeconds(TickInterval))
                .Subscribe(_ => Tick())
                .AddTo(_disposables);
        }

        private void Tick()
        {
            ProductionLogics();
            TransferLogics();
        }
        
        private void TransferLogics()
        {
            if(!_isCollecting || _collector == null)
                return;
            
            float rate = _settings.TransferSpeed?.Evaluate(_collectSeconds) ?? 1f;
            
            if (rate < 0f) 
                rate = 0f;
            
            _transferAccumulator += rate;

            int toTransfer = (int)Math.Floor(_transferAccumulator);
            
            if (toTransfer > 0)
            {
                int removed = _data.RemoveCount(toTransfer);
                
                if (removed > 0)
                {
                    _transferAccumulator -= removed;
                    _collector.AddResource(_data.ResourcesType, removed);
                }
            }

            _collectSeconds += TickInterval;
        }
        
        private void ProductionLogics()
        {
            _fractionAccumulator += _settings.ProductionPerSecond * TickInterval;

            if(!(_fractionAccumulator > 0))
                return;
            
            int wholeToAdd = (int) Math.Floor(_fractionAccumulator);
            
            if(wholeToAdd <= 0)
                return;
            
            _fractionAccumulator -= wholeToAdd;
            _data.AddCount(wholeToAdd);
        }

        public void OnPlayerStartCollectResources(Player player)
        {
            _collector = player;
            player.StartCollect();
            _isCollecting = true;
            _collectSeconds = 0f;
        }

        public void OnPlayerStopCollectResources(Player player)
        {
            if(_collector != player)
                return;
            
            player.StopCollect();
            _isCollecting = false;
            _collector = null;
            _collectSeconds = 0f;
            _transferAccumulator = 0f;
        }

        public void Dispose() =>
            _disposables.Dispose();
    }
}