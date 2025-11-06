using Constants;
using GameObjects.ResourcesSpace;
using Infrastructure.Loggers;
using UniRx;

namespace GameObjects.Players
{
    public class Player
    {
        private readonly PlayerData _data;
        private readonly IGameLogger _logger;

        public IReadOnlyReactiveProperty<bool> IsCollected => _isCollected;
        private readonly ReactiveProperty<bool> _isCollected = new(false);

        public Player(PlayerData data, IGameLogger logger)
        {
            _data = data;
            _logger = logger;
        }

        public void StartCollect() =>
            _isCollected.Value = true;

        public void StopCollect() =>
            _isCollected.Value = false;

        public void AddResource(GameResourcesType dataResourcesType, int added)
        {
            _logger.Log($"Collect {dataResourcesType} {added}", LogLevel.Info, LogSystemType.Player, LogIds.Player.CollectResource);
            _data.AddResource(dataResourcesType, added);
        }
    }
}