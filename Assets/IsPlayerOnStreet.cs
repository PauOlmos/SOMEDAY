using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerOnStreet : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOnStreet = false;
    public Rigidbody player;
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
            
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            isOnStreet = true;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            player.AddForce(transform.up * 5, ForceMode.Impulse);

            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            isOnStreet = true;
        }
    }

}
