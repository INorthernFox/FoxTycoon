namespace Infrastructure.SaveServices.Interfaces
{
    public interface ISaveManager
    {
        public void Load();
        public void SaveAll();
        public void SaveSettings();

        public void RegisterSaveable(ISaveable saveable);

        public void RemoveSaveable(ISaveable saveable);
    }
}