using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateHand : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hand;
    public bool moveHand = false;
    public float speed = 20.0f;
    public Transform rotateAroundPosition;
    public GameObject handDamage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (moveHand == true)
        {
            hand.transform.RotateAround(rotateAroundPosition.position, Vector3.forward ,-Time.deltaTime * speed);
            //hand.transform.Translate(-Vector3.forward * Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "PlayerModel")
        {
            moveHand = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.name == "PlayerModel")
        {
            moveHand = true;
        }
    }
}
