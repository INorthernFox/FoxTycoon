using UniRx;

namespace GameObjects.ResourcesSpace
{
    public interface IResourcesData
    {
        IReadOnlyReactiveProperty<int> Count { get; }
        GameResourcesType Type { get; }
    }
}