using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossProjectile : MonoBehaviour
{
    // Start is called before the first frame update

    public int projectileType;

    public GameObject finalDog;
    public GameObject liquid;
    public GameObject player;

    void Start()
    {
        projectileType = Random.Range(0, 3);
        //projectileType = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            switch (projectileType)
            {
                case 0:

                    GameObject dog = Instantiate(finalDog, gameObject.transform.position, Quaternion.identity);

                    dog.GetComponent<ShadowDog>().player = player;

                    break;

                case 1:

                    GameObject likid = Instantiate(liquid, gameObject.transform.position, Quaternion.identity);
                    likid.transform.position = new Vector3(likid.transform.position.x, -220.0f, likid.transform.position.z);
                    break;

                default: break;
            }

            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            switch (projectileType)
            {
                case 0:

                    GameObject dog = Instantiate(finalDog, gameObject.transform.position, Quaternion.identity);

                    dog.GetComponent<ShadowDog>().player = player;

                    break;

                case 1:

                    Instantiate(liquid, gameObject.transform.position, Quaternion.identity);

                    break;

                default: break;
            }

            Destroy(gameObject);
        }
    }
}
