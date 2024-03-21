using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isPlayerAtHome : MonoBehaviour
{
    public bool atHome = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision other)
    {

        if (other.gameObject.name == "Player")
        {
            atHome = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            gameObject.GetComponent<BoxCollider>().isTrigger = true;

            atHome = true;
        }
    }
}
