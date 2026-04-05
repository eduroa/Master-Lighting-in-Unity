using UnityEngine;

public class RunAnimationScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            _animator.SetFloat("RunAnimation", 1.0f);
        }

        if (Input.GetKey(KeyCode.X))
        {
            _animator.SetFloat("RunAnimation", 0.0f);
        }

    }
}
