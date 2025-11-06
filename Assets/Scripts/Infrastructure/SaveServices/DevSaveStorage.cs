using Infrastructure.SaveServices.Interfaces;
using UnityEngine;

namespace Infrastructure.SaveServices
{
    public class DevSaveStorage : ISaveStorage
    {
        public const string SaveKey = "dev_save";
        
        public void Write(string data) =>
            PlayerPrefs.SetString(SaveKey, data);

        public string Read() =>
            PlayerPrefs.GetString(SaveKey);
    }
}