using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerOnGraveyard : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody player;
    public bool isAtGraveyard = false;
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
            isAtGraveyard = true;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            player.AddForce(transform.up * 5, ForceMode.Impulse);
            isAtGraveyard = true;
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name == "Player")
        {
            isAtGraveyard = true;
            Destroy(gameObject);
        }
    }
}
