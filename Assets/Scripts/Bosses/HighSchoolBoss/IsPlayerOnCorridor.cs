using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerOnCorridor : MonoBehaviour
{
    public bool isOnCorridor = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isOnCorridor = true;
        }    
        else Debug.Log(other.gameObject.name);
    }

}
