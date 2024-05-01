using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCar : MonoBehaviour
{
    public GameObject car;
    public bool moveCar = false;
    public float speed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(moveCar == true)
        {
            car.transform.Translate(-Vector3.forward * Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "PlayerModel")
        {
            moveCar = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.name == "PlayerModel")
        {
            moveCar = true;
        }
    }

}
