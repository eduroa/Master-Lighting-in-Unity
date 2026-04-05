using UnityEngine;

[ExecuteAlways]
public class StandingLight_LightsController : MonoBehaviour
{
    [Header("Lamp State")]
    public bool _isOn = false;   // Toggle in Inspector

    [Header("Light Objects")]
    public GameObject _setLights;

    [Header("Emissive Objects")]
    public Renderer _LightBulb;   // Shader: MasterLightCourseShader_v1
    public Renderer _LightReflector;   // Shader: MLC_CourtYard_Standard

    [Header("Emissive Settings")]
    [ColorUsage(true, true)]
    public Color _emissiveOn = Color.white * 6f;   // HDR intensity
    [ColorUsage(true, true)]
    public Color _emissiveOff = Color.black;

    private MaterialPropertyBlock _mpbA;
    private MaterialPropertyBlock _mpbB;

    void Awake()
    {
        _mpbA = new MaterialPropertyBlock();
        _mpbB = new MaterialPropertyBlock();
    }

    void OnValidate()
    {
        if (_mpbA == null) _mpbA = new MaterialPropertyBlock();
        if (_mpbB == null) _mpbB = new MaterialPropertyBlock();

        if (_isOn)
            TurnOn();
        else
            TurnOff();
    }

    public void TurnOn()
    {
        if (_setLights != null)
            _setLights.SetActive(true);

        if (_LightBulb == null) return;
        _LightBulb.GetPropertyBlock(_mpbA);
        _mpbA.SetColor("_Emissive", _emissiveOn);
        _LightBulb.SetPropertyBlock(_mpbA);

        if (_LightReflector == null) return;
        _LightReflector.GetPropertyBlock(_mpbB);
        _mpbB.SetColor("_Multiply_Emissive_Color", _emissiveOn);
        _LightReflector.SetPropertyBlock(_mpbB);

    }

    public void TurnOff()
    {
        if (_setLights != null)
            _setLights.SetActive(false);

        if (_LightBulb == null) return;
        _LightBulb.GetPropertyBlock(_mpbA);
        _mpbA.SetColor("_Emissive", _emissiveOff);
        _LightBulb.SetPropertyBlock(_mpbA);

        if (_LightReflector == null) return;
        _LightReflector.GetPropertyBlock(_mpbB);
        _mpbB.SetColor("_Multiply_Emissive_Color", _emissiveOff);
        _LightReflector.SetPropertyBlock(_mpbB);

    }
}

