using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Infrastructure.SceneManagementServices.Editor
{
    [CustomEditor(typeof(SceneSettings))]
    public class SceneSettingsEditor : UnityEditor.Editor
    {
        private SerializedProperty _scenesProp;
        private bool _addIfMissing = true;

        private void OnEnable()
        {
            _scenesProp = serializedObject.FindProperty("_scenes");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Синхронизация BuildIndex", EditorStyles.boldLabel);
            _addIfMissing = EditorGUILayout.ToggleLeft(
                "Добавлять сцену в Build Settings, если её там нет",
                _addIfMissing);

            using (new EditorGUI.DisabledScope(_scenesProp == null || _scenesProp.arraySize == 0))
            {
                if (GUILayout.Button("Установить сцены"))
                {
                    ApplyBuildIndexes(_scenesProp, _addIfMissing);
                }
            }

            EditorGUILayout.HelpBox(
                "Кнопка пробежится по списку, возьмёт SceneAsset из поля 'Screen', " +
                "найдёт (или добавит) сцену в Build Settings и проставит BuildIndex. " +
                "Если Name пустое — подставит имя файла сцены.",
                MessageType.Info);

            serializedObject.ApplyModifiedProperties();
        }

        private static void ApplyBuildIndexes(SerializedProperty scenesProp, bool addIfMissing)
        {
            SceneSettings targetSettings = (SceneSettings)scenesProp.serializedObject.targetObject;

            List<EditorBuildSettingsScene> buildScenes = EditorBuildSettings.scenes.ToList();
            int updated = 0, addedToBuild = 0, skipped = 0;

            for (int i = 0; i < scenesProp.arraySize; i++)
            {
                var element = scenesProp.GetArrayElementAtIndex(i);

                var nameProp       = element.FindPropertyRelative("Name");
                var buildIndexProp = element.FindPropertyRelative("BuildIndex");
                var screenProp     = element.FindPropertyRelative("Screen");

                var sceneAsset = screenProp?.objectReferenceValue as SceneAsset;
                if (sceneAsset == null)
                {
                    skipped++;
                    continue;
                }

                var path = AssetDatabase.GetAssetPath(sceneAsset);
                if (string.IsNullOrEmpty(path))
                {
                    skipped++;
                    continue;
                }

                int indexInBuild = IndexOfPath(buildScenes, path);
                if (indexInBuild < 0 && addIfMissing)
                {
                    buildScenes.Add(new EditorBuildSettingsScene(path, true));
                    EditorBuildSettings.scenes = buildScenes.ToArray();
                    indexInBuild = IndexOfPath(buildScenes, path);
                    if (indexInBuild >= 0) addedToBuild++;
                }

                if (indexInBuild >= 0)
                {
                    buildIndexProp.intValue = indexInBuild;

                    if (string.IsNullOrEmpty(nameProp.stringValue))
                        nameProp.stringValue = System.IO.Path.GetFileNameWithoutExtension(path);

                    updated++;
                }
                else
                {
                    buildIndexProp.intValue = -1; 
                }
            }

            scenesProp.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(targetSettings);
            AssetDatabase.SaveAssets();

            Debug.Log($"[SceneSettings] Индексы обновлены. Обновлено: {updated}, добавлено в Build Settings: {addedToBuild}, пропущено: {skipped}");
        }

        private static int IndexOfPath(List<EditorBuildSettingsScene> list, string path)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i].path == path) return i;
            return -1;
        }
    }
}