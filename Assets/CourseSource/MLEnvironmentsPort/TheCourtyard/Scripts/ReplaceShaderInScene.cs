using UnityEngine;
using UnityEditor;

public class ReplaceShaderInScene : EditorWindow
{
    private Shader shaderToFind;
    private Shader shaderToReplace;

    [MenuItem("Tools/Master Lighting - Shaders/Replace Shader In Scene")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceShaderInScene>("Replace Shader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace Shader in All MeshRenderers", EditorStyles.boldLabel);

        shaderToFind = (Shader)EditorGUILayout.ObjectField("Shader to Find", shaderToFind, typeof(Shader), false);
        shaderToReplace = (Shader)EditorGUILayout.ObjectField("Shader to Replace With", shaderToReplace, typeof(Shader), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("Run Replacement"))
        {
            if (shaderToFind == null || shaderToReplace == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign both shaders.", "OK");
                return;
            }

            ReplaceShaders();
        }
    }

    private void ReplaceShaders()
    {
        int count = 0;

        MeshRenderer[] renderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

        Undo.RecordObjects(renderers, "Replace Shaders");

        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials;
            bool modified = false;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] != null && materials[i].shader == shaderToFind)
                {
                    materials[i].shader = shaderToReplace;
                    modified = true;
                    count++;
                }
            }

            if (modified)
            {
                renderer.sharedMaterials = materials;
                EditorUtility.SetDirty(renderer);
            }
        }

        EditorUtility.DisplayDialog("Done", $"Replaced {count} materials.", "OK");
    }
}

