using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.RuntimeConsole.EditorAddons
{
    public class RuntimeConsoleMenu
    {
        [MenuItem("Better/Runtime Console/Add to active scene")]
        public static void AddConsoleToActiveScene()
        {
            var path = AssetDatabase.GUIDToAssetPath("a5e16754ea9e075458a77e79b6c2b72d");
            var prefab =  AssetDatabase.LoadAssetAtPath<Object>(path);
            var activeScene = SceneManager.GetActiveScene();
            PrefabUtility.InstantiatePrefab(prefab, activeScene);
            EditorSceneManager.MarkSceneDirty(activeScene);
        }
    }
}