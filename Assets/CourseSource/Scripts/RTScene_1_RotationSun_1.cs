using UnityEngine;

public class RTScene_1_RotationSun_1 : MonoBehaviour
{
    public float minAngle = 60f;
    public float maxAngle = 140f;
    public float rotationSpeed = 60f; // degrees per second

    private float currentAngle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize from current rotation
        currentAngle = transform.localRotation.eulerAngles.x;
        if (currentAngle > 180f) currentAngle -= 360f;
        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

    }

    // Update is called once per frame
    void Update()
    {
        float delta = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.X))
        {
            currentAngle += delta;
        }

        if (Input.GetKey(KeyCode.C))
        {
            currentAngle -= delta;
        }

        currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);

        // Apply rotation using Quaternion
        transform.localRotation = Quaternion.Euler(currentAngle, 0f, 0f);


    }
}
