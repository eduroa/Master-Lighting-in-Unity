using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class FixMaterial_CY_Standard_Specular : MonoBehaviour
{
    [MenuItem("Tools/Master Lighting - Shaders/Fix Empty Textures MLC_CourtYard_Standard_Specular Material")]
    private static void FixCourtYardMaterials()
    {
        // Load default textures from Resources folder
        Texture T_CY_White = AssetDatabase.LoadAssetAtPath<Texture>("Assets/MLEnvironmentsPort/TheCourtyard/Textures/T_CY_White.png");
        Texture T_CY_Normal = AssetDatabase.LoadAssetAtPath<Texture>("Assets/MLEnvironmentsPort/TheCourtyard/Textures/T_CY_Normal.png");
        Texture T_CY_Black = AssetDatabase.LoadAssetAtPath<Texture>("Assets/MLEnvironmentsPort/TheCourtyard/Textures/T_CY_Black.png");

        if (!T_CY_White || !T_CY_Normal || !T_CY_Black)
        {
            Debug.LogError("One or more required textures are missing in the path.");
            return;
        }

        int fixedCount = 0;

        // Scan all renderers in the scene
        foreach (var renderer in Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None))
        {
            foreach (var mat in renderer.sharedMaterials)
            {
                if (mat == null) continue;
                if (mat.shader == null) continue;

                if (mat.shader.name != "Shader Graphs/MLC_CourtYard_StandardSpecular")
                    continue;

                Undo.RecordObject(mat, "Fix CourtYard Material");

                // Fix missing textures
                FixTexture(mat, "_MainTex", T_CY_White);
                FixTexture(mat, "_SpecGlossMap", T_CY_Black);
                FixTexture(mat, "_BumpMap", T_CY_Normal);
                FixTexture(mat, "_OcclusionMap", T_CY_White);
                FixTexture(mat, "_EmissionMap", T_CY_Black);

                // Fix Occlusion Strength
                if (mat.HasProperty("_OcclusionStrength"))
                {
                    if (Mathf.Approximately(mat.GetFloat("_OcclusionStrength"), 0f))
                        mat.SetFloat("_OcclusionStrength", 1f);
                }

                fixedCount++;
            }
        }

        Debug.Log($"MLC_CourtYard_Standard_Specular Fix Complete. Updated {fixedCount} materials.");
    }

    private static void FixTexture(Material mat, string property, Texture defaultTex)
    {
        if (!mat.HasProperty(property))
            return;

        Texture current = mat.GetTexture(property);

        if (current == null)
            mat.SetTexture(property, defaultTex);
    }


}
