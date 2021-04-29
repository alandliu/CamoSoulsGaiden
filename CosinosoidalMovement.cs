using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosinosoidalMovement : MonoBehaviour
{
    [SerializeField]
    float frequency = 0.5f;

    [SerializeField]
    float magnitude = 0.5f;

    Vector3 pos;


    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Oscillate();
    }

    void Oscillate()
    {
        transform.position = pos + transform.up * Mathf.Cos(Time.time * frequency) * magnitude;
    }
}
