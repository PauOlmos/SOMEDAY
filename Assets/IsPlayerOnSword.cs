using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerOnSword : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOnSwordThePlayer = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "PlayerModel")
        {
            isOnSwordThePlayer = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerModel")
        {
            isOnSwordThePlayer = true;
        }
    }

}
