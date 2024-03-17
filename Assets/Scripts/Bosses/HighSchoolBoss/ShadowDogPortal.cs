using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShadowDogPortal : MonoBehaviour
{
    public GameObject shadowDogPrefab;
    public GameObject player;
    public float shadowDogPortalTimer = 0.0f;
    public int numDogsSpawn = 0;
    public int maxDogsSpawn;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        shadowDogPortalTimer += Time.deltaTime;
        if(shadowDogPortalTimer > 1.5f)
        {
            GameObject dog = Instantiate(shadowDogPrefab, new Vector3(gameObject.transform.position.x,0.35f,gameObject.transform.position.z),Quaternion.identity);
            dog.GetComponent<NavMeshAgent>().destination = player.transform.position;
            dog.GetComponent<ShadowDog>().player = player;
            numDogsSpawn++;
            shadowDogPortalTimer = 0.0f;
        }
        if(maxDogsSpawn <= numDogsSpawn)
        {
            Destroy(gameObject);
        }
    }
}
