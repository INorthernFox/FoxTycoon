using System;
using UniRx;

namespace GameObjects.ResourcesSpace
{
    [Serializable]
    public class ResourcesData : IResourcesData
    {
        public GameResourcesType Type { get; }

        public IReadOnlyReactiveProperty<int> Count => _count;

        private readonly ReactiveProperty<int> _count = new(0);

        public ResourcesData(GameResourcesType type , int count)
        {
            Type = type;
            _count.Value = count;
        }

        public void Initialize(int count) => 
            _count.Value = count;

        public int AddCount(int count)
        {
            if(count <= 0)
                return 0;
            
            _count.Value += count;
            return count;
        }

        public int RemoveCount(int count)
        {
            if(count <= 0)
                return 0;

            if(_count.Value - count <= 0)
            {
                int result = _count .Value;
                _count.Value = 0;
                return result;
            }
            
            _count.Value -= count;
            return count;
        }

        public struct Save
        {
            public readonly GameResourcesType Type;
            public readonly int Count;
            
            public Save(GameResourcesType type, int count)
            {
                Type = type;
                Count = count;
            }
        }
    }
   
}