using UnityEngine;
using UnityEngine.Rendering;

public class RTScene_1_FlickeringLight_1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Light _pointlight;
    public float minIntensity = 300f;
    public float maxIntensity = 8000f;
    public float flickerSpeed = 0.1f; // seconds between flickers

    private float timer;

    void Start()
    {
        if (_pointlight == null)
            _pointlight = GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= flickerSpeed)
        {
            timer = 0f;

            //float _i = Random.Range(300f, 8000f);
            // Optional: sinusoidal flicker for smoother effect
            float t = Mathf.PingPong(Time.time * 5f, 1f);
            float _i = Mathf.Lerp(minIntensity, maxIntensity, t);
            _pointlight.intensity = LightUnitUtils.ConvertIntensity(_pointlight, _i, LightUnit.Lumen ,LightUnitUtils.GetNativeLightUnit(_pointlight.type));
            
        }

    }
}
