using UniRx;

namespace Infrastructure.SaveServices.Interfaces
{
    public interface ISaveable
    {
        public string Key { get; }
        public bool SaveAlways { get; }
        public IReadOnlyReactiveProperty<bool> HaveChanges { get; }
        public string Save();
        public void Load(string save);
    }

}