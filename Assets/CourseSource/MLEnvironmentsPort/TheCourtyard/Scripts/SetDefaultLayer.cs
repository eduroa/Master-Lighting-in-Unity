using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


public class SetDefaultLayer : MonoBehaviour
{
    [MenuItem("Tools/Master Lighting - Shaders/Set MeshRenderer Layers to Default")]
    private static void SetMeshRendererLayers()
    {
        int defaultLayer = LayerMask.NameToLayer("Default");
        int changedCount = 0;

        // Unity 6+ API: Find all MeshRenderers in scene
        var renderers = Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

        foreach (var renderer in renderers)
        {
            GameObject go = renderer.gameObject;

            if (go.layer != defaultLayer)
            {
                Undo.RecordObject(go, "Set Layer to Default");
                go.layer = defaultLayer;
                changedCount++;
            }
        }

        Debug.Log($"Set {changedCount} MeshRenderer GameObjects to 'Default' layer.");
    }
}

