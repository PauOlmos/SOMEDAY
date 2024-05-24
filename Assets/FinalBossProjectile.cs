using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalBossProjectile : MonoBehaviour
{
    // Start is called before the first frame update

    public int projectileType;

    public GameObject finalDog;
    public GameObject liquid;
    public GameObject player;
    public int difficulty;
    public float[] dogsSpeed = { 5, 7.5f, 8.5f };
    public float[] liquidsTimes = { 30.0f, 45.0f, 55.0f };
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
                    dog.GetComponent<NavMeshAgent>().speed = dogsSpeed[difficulty];

                    break;

                case 1:

                    GameObject likid = Instantiate(liquid, gameObject.transform.position, Quaternion.identity);
                    likid.transform.position = new Vector3(likid.transform.position.x, -220.0f, likid.transform.position.z);
                    likid.GetComponent<DieByTime>().deathTime = liquidsTimes[difficulty];
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
