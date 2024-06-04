using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothLight : MonoBehaviour
{

    public bool turnOff;

    public Light light;

    public float actualIntensity;
    public float desiredIntensity;
    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light>();         
        actualIntensity = light.intensity;

        if (turnOff == false) light.intensity = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (turnOff && light.intensity > desiredIntensity) light.intensity -= Time.deltaTime * actualIntensity / 3.0f;//TEST
        else if (turnOff == false && light.intensity < desiredIntensity) light.intensity += desiredIntensity * Time.deltaTime / 3.0f;//TEST
    }
}
