namespace GameObjects.ResourcesSpace
{
    public struct ResourceAddedInfo
    {
        public readonly GameResourcesType Type;
        public readonly int Count;

        public ResourceAddedInfo(GameResourcesType type, int count)
        {
            Count = count;
            Type = type;
        }
    }
}