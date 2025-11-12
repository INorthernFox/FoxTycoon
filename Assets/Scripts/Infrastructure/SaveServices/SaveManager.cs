using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Loggers;
using Infrastructure.SaveServices.Interfaces;
using Newtonsoft.Json;
using UniRx;
using Constants;

namespace Infrastructure.SaveServices
{
    public class SaveManager : ISaveManager, IDisposable
    {
        private readonly ISaveStorage _saveStorage;
        private readonly IGameLogger _logger;

        private readonly Dictionary<string, ISaveable> _saveables = new();
        private readonly Dictionary<string, ISaveable> _settings = new();
        private readonly Dictionary<string, ISaveable> _alwaysSaveables = new();
        private readonly Dictionary<string, IDisposable> _streams = new();

        private readonly HashSet<ISaveable> _toSave = new();

        private Dictionary<string, string> _saves = new();

        public SaveManager(
            ISaveStorage saveStorage,
            IGameLogger logger)
        {
            _saveStorage = saveStorage;
            _logger = logger;
        }

        public void Load()
        {
            _logger.Log("Start Loading Saves", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.LoadStart);

            string save = _saveStorage.Read();

            _logger.Log("Saves read", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.LoadRead);

            if(string.IsNullOrEmpty(save))
            {
                _saves = new Dictionary<string, string>();
                _logger.Log("Saves is empty", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.LoadEmpty);
                return;
            }

            try
            {
                _saves = JsonConvert.DeserializeObject<Dictionary<string, string>>(save);

                if(_saves == null)
                {
                    _logger.Log("Failed to deserialize saves - result is null", LogLevel.Error, LogSystemType.Save, LogIds.SaveManager.LoadDeserialized);
                    _saves = new Dictionary<string, string>();
                    return;
                }

                _logger.Log($"Saves deserialized {_saves.Count}", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.LoadDeserialized);

                Load(_alwaysSaveables.Values, _saveables.Values, _settings.Values);

                _logger.Log("Saves loaded", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.LoadComplete);
            }
            catch(Exception e)
            {
                _logger.Log($"Failed to deserialize saves: {e.Message}", LogLevel.Error, LogSystemType.Save, LogIds.SaveManager.LoadDeserialized);
                _saves = new Dictionary<string, string>();
            }
        }

        private void Load(params IEnumerable<ISaveable>[] collections)
        {
            foreach(IEnumerable<ISaveable> collection in collections)
            {
                foreach(ISaveable item in collection.Where(x => _saves.ContainsKey(x.Key)))
                {
                    string data = _saves[item.Key];
                    try
                    {
                        item.Load(data);
                    }
                    catch(Exception e)
                    {
                        _logger.Log($"Failed to load saveable '{item.Key}': {e.Message}", LogLevel.Error, LogSystemType.Save, LogIds.SaveManager.LoadDeserialized);
                        _saves.Remove(item.Key);
                    }
                }
            }
        }

        public void SaveAll() =>
            Save(_alwaysSaveables.Values, _toSave);

        public void SaveSettings() =>
            Save(_settings.Values);

        private void Save(params IEnumerable<ISaveable>[] collections)
        {
            _logger.Log($"Start Save {collections.Length}", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.SaveStart);

            foreach(IEnumerable<ISaveable> collection in collections)
            {
                foreach(ISaveable item in collection)
                {
                    string data = item.Save();
                    _saves[item.Key] = data;
                }
            }

            _logger.Log("Start Serialize Save", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.SerializeStart);

            string json = JsonConvert.SerializeObject(_saves);

            _logger.Log("Start Write Save", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.WriteStart);

            _saveStorage.Write(json);

            _logger.Log("Saved", LogLevel.Info, LogSystemType.Save, LogIds.SaveManager.SaveComplete);
        }

        public void RegisterSaveable(ISaveable saveable)
        {
            if(saveable is ISettingsSaveable)
                AddToSettings(saveable);
            else if(saveable.SaveAlways)
                AddToSaveAlways(saveable);
            else
                AddToBase(saveable);

            if(_saves.TryGetValue(saveable.Key, out string save))
            {
                try
                {
                    saveable.Load(save);
                }
                catch(Exception e)
                {
                    _logger.Log($"Failed to load saveable on register '{saveable.Key}': {e.Message}", LogLevel.Error, LogSystemType.Save, LogIds.SaveManager.LoadDeserialized);
                    _saves.Remove(saveable.Key);
                }
            }
        }

        private void AddToSettings(ISaveable saveable)
        {
            _settings.Add(saveable.Key, saveable);
            IDisposable stream = saveable.HaveChanges
                .Where(x => x)
                .Subscribe(_ => SaveSettings());

            _streams.Add(saveable.Key, stream);
        }

        private void AddToBase(ISaveable saveable)
        {
            IDisposable stream = saveable.HaveChanges
                .Where(x => x)
                .Subscribe(_ => _toSave.Add(saveable));

            _streams.Add(saveable.Key, stream);
            _saveables.Add(saveable.Key, saveable);
        }

        private void AddToSaveAlways(ISaveable saveable) =>
            _alwaysSaveables.Add(saveable.Key, saveable);

        public void RemoveSaveable(ISaveable saveable)
        {
            string key = saveable.Key;

            _alwaysSaveables.Remove(key);
            _saveables.Remove(key);
            _settings.Remove(key);

            if(_streams.TryGetValue(key, out IDisposable stream))
            {
                stream.Dispose();
                _streams.Remove(key);
            }

            _saves.Remove(key);
            _toSave.Remove(saveable);
        }

        public void Dispose()
        {
            foreach(IDisposable item in _streams.Values)
                item.Dispose();

            _streams.Clear();
            _saves.Clear();
            _alwaysSaveables.Clear();
            _saveables.Clear();
            _toSave.Clear();
            _settings.Clear();
        }
    }
}