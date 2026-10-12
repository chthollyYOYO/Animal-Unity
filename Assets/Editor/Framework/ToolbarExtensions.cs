using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

namespace GameEditor
{
    public static class ToolbarExtensions
    {
        [InitializeOnLoadMethod]
        private static void AddListener()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnLeftToolbarGUI);
            Application.quitting += OnQuitting;
        }

        private static void OnLeftToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("开始游戏"), GUILayout.Width(60)))
            {
                var startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
                EditorSceneManager.playModeStartScene = startScene;
                EditorApplication.isPlaying = true;
            }
        }

        private static void OnQuitting()
        {
            EditorSceneManager.playModeStartScene = null;
        }
    }
}