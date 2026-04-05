using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;


public class CleanMissingScript : MonoBehaviour
{
    // -------------------------------
    // 1. Clean active scene
    // -------------------------------
    [MenuItem("Tools/Master Lighting - Shaders/Remove Missing Scripts (Scene + Prefabs)")]
    private static void CleanAll()
    {
        CleanScene();
        CleanPrefabs();

        Debug.Log("Cleanup complete: Scene and Prefabs done.");
    }

    private static void CleanScene()
    {
        var roots = UnityEngine.SceneManagement.SceneManager
            .GetActiveScene()
            .GetRootGameObjects();

        int missing = 0;

        foreach (var root in roots)
        {
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
                if (count > 0)
                {
                    Undo.RegisterCompleteObjectUndo(t.gameObject, "Remove Missing Scripts");
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(t.gameObject);
                    missing += count;
                }
            }
        }

        Debug.Log($"Scene cleanup: Removed {missing} missing scripts.");
    }

    // -------------------------------
    // 2. Clean all prefabs in project
    // -------------------------------
    private static void CleanPrefabs()
    {
        string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
        int totalMissing = 0;

        foreach (string guid in prefabGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab == null)
                continue;

            int removed = CleanPrefab(prefab);

            if (removed > 0)
            {
                totalMissing += removed;
                EditorUtility.SetDirty(prefab);
            }
        }

        if (totalMissing > 0)
            AssetDatabase.SaveAssets();

        Debug.Log($"Prefab cleanup: Removed {totalMissing} missing scripts.");
    }

    private static int CleanPrefab(GameObject prefab)
    {
        int missing = 0;

        // Load prefab contents (safe for nested prefabs)
        GameObject root = PrefabUtility.LoadPrefabContents(AssetDatabase.GetAssetPath(prefab));

        foreach (var t in root.GetComponentsInChildren<Transform>(true))
        {
            int count = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject);
            if (count > 0)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(t.gameObject);
                missing += count;
            }
        }

        // Save back to prefab
        PrefabUtility.SaveAsPrefabAsset(root, AssetDatabase.GetAssetPath(prefab));
        PrefabUtility.UnloadPrefabContents(root);

        return missing;
    }
}

