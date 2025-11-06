using UnityEditor;
using UnityEngine;

namespace Infrastructure.SaveServices.Editor
{
    public static class DevSaveTools
    {
        [MenuItem("Tools/Dev Save/Clear Save")] 
        public static void ClearDevSave()
        {
            PlayerPrefs.DeleteKey(DevSaveStorage.SaveKey);
            PlayerPrefs.Save();
            Debug.Log($"[DevSave] Deleted PlayerPrefs key '{DevSaveStorage.SaveKey}'.");
        }
    }
}