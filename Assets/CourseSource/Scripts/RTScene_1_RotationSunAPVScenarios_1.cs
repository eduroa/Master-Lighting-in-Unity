using UnityEngine;

public class RTScene_1_RotationSunAPVScenarios_1 : MonoBehaviour
{
    public float minAngle = 60f;
    public float maxAngle = 140f;
    public float rotationSpeed = 60f; // degrees per second

    UnityEngine.Rendering.ProbeReferenceVolume probeRefVolume;
    private int numberOfCellsBlendedPerFrame = 10;    

    private float currentAngle;
    private float interpolateValue;
    public bool ActivateBlendScenarios = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    float setRange(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        return newMin + (((value - oldMin) / (oldMax - oldMin)) * (newMax - newMin));
    }

    void Start()
    {

        currentAngle = 60.0f;
        Debug.Log("Start Current Angle " + currentAngle.ToString());
        interpolateValue = setRange(currentAngle, minAngle, maxAngle, 0.0f, 1.0f);

        probeRefVolume = UnityEngine.Rendering.ProbeReferenceVolume.instance;
        probeRefVolume.lightingScenario = "Angle_1";
        probeRefVolume.numberOfCellsBlendedPerFrame = numberOfCellsBlendedPerFrame;


    }

    // Update is called once per frame
    void Update()
    {
        float delta = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.X))
        {
            currentAngle += delta;
            interpolateValue = setRange(currentAngle, minAngle, maxAngle, 0.0f, 1.0f);
            if (ActivateBlendScenarios) APVChange();
        }

        if (Input.GetKey(KeyCode.C))
        {
            currentAngle -= delta;
            interpolateValue = setRange(currentAngle, minAngle, maxAngle, 0.0f, 1.0f);
            if (ActivateBlendScenarios) APVChange();
        }

        
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
       // Debug.Log("Current Angle " + currentAngle.ToString());
       // Debug.Log("Interpolate Value " + interpolateValue.ToString());
        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);


    }

    void APVChange()
    {

        probeRefVolume.BlendLightingScenario("Angle_2", interpolateValue);
    }

}
