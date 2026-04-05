using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;




public class LightSwitch : MonoBehaviour
{

    [Header("Directional Light")]
    public Light sun;

    public enum EditorLightState
    {
        Afternoon,
        Sunset
    }
    public EditorLightState editorState = EditorLightState.Afternoon;
    public ReflectionProbeBlend RPBlend;

    private Vector3 afternoonRotation = new Vector3(63.285f, -74.777f, 2.01f);
    private Vector3 sunsetRotation = new Vector3(12.419f, -76.374f, 0.925f);
    private float transitionSpeed = 2f;
    private Quaternion targetRotation;

    [Header("Light Settings")]
    public Color afternoonFilter = new Color(1f, 0.98f, 0.9f);
    public Color sunsetFilter = new Color(1f, 0.94f, 0.9f);

    public float afternoonTemp = 8000f;
    public float sunsetTemp = 4000f;

    public float afternoonIntensity = 100000f;
    public float sunsetIntensity = 2000f;

    [Header("Intensity Curve")]
    public AnimationCurve intensityCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Header("Filter Color Curve")]
    public AnimationCurve ColorCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Header("Temperature Curve")]
    public AnimationCurve TempCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Header("Exposure Curve")]
    public AnimationCurve exposureCurve = AnimationCurve.Linear(0, 0, 1, 1);
    private AnimationCurve APVCurve = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Volume")]
    public Volume exposureVolume;
    public Volume afternoonVolume;
    public Volume sunsetVolume;

    private float stepSize = 0.02f;
    private float t = 0f; // 0 = full afternoon, 1 = full sunset

    [Header("APV")]
    public string afternoonScenarioID = "Afternoon" ;
    public string sunsetScenarioID = "Sunset";

    UnityEngine.Rendering.ProbeReferenceVolume probeRefVolume;
    private int numberOfCellsBlendedPerFrame = 10;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Start in afternoon state
        targetRotation = Quaternion.Euler(afternoonRotation);
        sun.transform.rotation = targetRotation;

        t = 0f;
        ApplyState();


    }

#if UNITY_EDITOR
    void OnValidate()
    {
        //probeRefVolume = ProbeReferenceVolume.instance;
        if (!Application.isPlaying)
        {
            switch (editorState)
            {
                case EditorLightState.Afternoon:
                    t = 0f;                    
                    ApplyState();
                    break;

                case EditorLightState.Sunset:
                    t = 1f;
                    ApplyState();
                    break;

            }
        }
    }
#endif


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            t += stepSize;
            t = Mathf.Clamp01(t);
            ApplyState();
        }

        // A → move towards afternoon (decrease t)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            t -= stepSize;
            t = Mathf.Clamp01(t);
            ApplyState();
        }


    }

    void ApplyState()
    {
        ApplySunRotation();
        ApplyLightSettings();
        ApplyVolumenBlending();
        ApplyExposure();        
        ApplyAPVBlend();
        RPBlend.SetBlend(t);

    }

    void ApplyExposure()
    {
        //Exposure
        float expT = exposureCurve.Evaluate(t);
        float minEV = Mathf.Lerp(10f, 5.0f, expT);
        float maxEV = Mathf.Lerp(12f, 6.0f, expT);
        //Debug.Log("T = " + t.ToString() + " ExpT = " + expT.ToString() + " minEV = " + minEV.ToString() + " maxEV = " + maxEV.ToString());

        if (exposureVolume.profile.TryGet<Exposure>(out var exposure))
        {
            exposure.limitMin.value = minEV;
            exposure.limitMax.value = maxEV;
        }

    }

    void ApplyVolumenBlending()
    {
        // Volume blending
        afternoonVolume.weight = 1f - t;
        sunsetVolume.weight = t;
    }

    void ApplyLightSettings()
    {
        float cIntEvaluation = intensityCurve.Evaluate(t);
        float cColEvaluation = ColorCurve.Evaluate(t);
        float cTempEvaluation = TempCurve.Evaluate(t);

        sun.color = Color.Lerp(afternoonFilter, sunsetFilter, cColEvaluation);
        sun.colorTemperature = Mathf.Lerp(afternoonTemp, sunsetTemp, cTempEvaluation);
        sun.intensity = Mathf.Lerp(afternoonIntensity, sunsetIntensity, cIntEvaluation);
    }
    void ApplySunRotation()
    {
        Quaternion rotAfternoon = Quaternion.Euler(afternoonRotation);
        Quaternion rotSunset = Quaternion.Euler(sunsetRotation);
        Quaternion rot = Quaternion.Slerp(rotAfternoon, rotSunset, t);
        sun.transform.rotation = rot;
    }

    void ApplyAPVBlend()
    {
        probeRefVolume = ProbeReferenceVolume.instance;
        probeRefVolume.numberOfCellsBlendedPerFrame = numberOfCellsBlendedPerFrame;

        if (probeRefVolume == null)
            Debug.Log("Null APV in Blend");

        // Blend factor (0 = afternoon, 1 = sunset)
        //Debug.Log("APV Blend = " + t.ToString());
        float interpolateValue = APVCurve.Evaluate(t);
        //Debug.Log("Interpolate Value = " + interpolateValue.ToString());
        probeRefVolume.BlendLightingScenario("Sunset", interpolateValue);

    }


}


