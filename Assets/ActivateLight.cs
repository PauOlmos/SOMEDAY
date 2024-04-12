using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLight : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isActive;
    public Light asignedLight;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        asignedLight.enabled = isActive;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "PlayerModel")
        {
            isActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.name == "PlayerModel")
        {
            isActive = false;
        }
    }
}
