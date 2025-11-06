namespace GameObjects.ResourcesSpace.Extensions
{
    public static class ResourcesDataExtension
    {
        public static ResourcesData.Save ToSave(this ResourcesData data) =>
            new(data.Type, data.Count.Value);
        
        public static ResourcesData WithSave(this ResourcesData.Save data) =>
            new(data.Type, data.Count);
    }
}