using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsRecon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Find all lights in the scene
        Light[] allLights = FindObjectsOfType<Light>();

        // Iterate through each light and print its type and name
        foreach (Light light in allLights)
        {
            string lightType = light.type.ToString();
            string lightName = light.name;
            Debug.Log($"Light Name: {lightName}, Light Type: {lightType}");
        }
    }
}
