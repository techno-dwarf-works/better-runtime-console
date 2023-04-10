using Better.RuntimeConsole.Runtime;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.RuntimeConsole.EditorAddons
{
    public class RuntimeConsoleMenu
    {
        private const string PrefabGuid = "a5e16754ea9e075458a77e79b6c2b72d";

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Add to Overlay")]
        public static void AddOverlayConsoleToActiveScene()
        {
            InstantiateConsoleToScene(RenderMode.ScreenSpaceOverlay);
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Add to Camera Space")]
        public static void AddCameraConsoleToActiveScene()
        {
            InstantiateConsoleToScene(RenderMode.ScreenSpaceCamera);
        }

        [MenuItem(BetterInternalTools.MenuItemPrefix + "/Add to World Space")]
        public static void AddWorldSpaceConsoleToActiveScene()
        {
            InstantiateConsoleToScene(RenderMode.WorldSpace);
        }

        private static void InstantiateConsoleToScene(RenderMode canvasRenderMode)
        {
            var path = AssetDatabase.GUIDToAssetPath(PrefabGuid);
            var prefab = AssetDatabase.LoadAssetAtPath<ConsoleInitializer>(path);
            var activeScene = SceneManager.GetActiveScene();
            var instance = PrefabUtility.InstantiatePrefab(prefab, activeScene) as ConsoleInitializer;
            var canvas = instance.GetComponentInChildren<Canvas>();
            canvas.renderMode = canvasRenderMode;
            if (canvasRenderMode == RenderMode.WorldSpace || canvasRenderMode == RenderMode.ScreenSpaceCamera)
            {
                var camera = Camera.main;
                if (camera == null)
                {
                    EditorUtility.DisplayDialog("Warning",
                        "No main camera in scene. Please set camera as event camera.", "Ok");

                    EditorGUIUtility.PingObject(canvas);
                }
                else
                {
                    canvas.worldCamera = camera;
                }
            }

            EditorSceneManager.MarkSceneDirty(activeScene);
        }
    }
}