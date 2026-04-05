using UnityEngine;
using UnityEditor;

public class ReplaceStandardWithHDRPShader : EditorWindow
{
    private Shader hdrpShader;

    [MenuItem("Tools/Master Lighting - Shaders/Replace Built-in Standard Shader")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceStandardWithHDRPShader>("Replace Built-in Standard Shader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace Built-in Standard Shader", EditorStyles.boldLabel);

        hdrpShader = (Shader)EditorGUILayout.ObjectField("HDRP Shader Graph", hdrpShader, typeof(Shader), false);

        if (GUILayout.Button("Replace in Scene"))
        {
            if (hdrpShader == null)
            {
                EditorUtility.DisplayDialog("Error", "Assign an HDRP Shader Graph shader.", "OK");
                return;
            }

            ReplaceShaders();
        }
    }

    private void ReplaceShaders()
    {
        int replaced = 0;

        MeshRenderer[] renderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);


        foreach (var renderer in renderers)
        {
            var materials = renderer.sharedMaterials;

            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] != null && materials[i].shader != null)
                {
                    // Built-in Standard Shader check
                    if (materials[i].shader.name == "Standard")
                    {
                        Undo.RecordObject(materials[i], "Replace Standard Shader");
                        materials[i].shader = hdrpShader;
                        replaced++;
                    }
                }
            }
        }

        EditorUtility.DisplayDialog("Done", $"Replaced {replaced} materials.", "OK");
    }
}