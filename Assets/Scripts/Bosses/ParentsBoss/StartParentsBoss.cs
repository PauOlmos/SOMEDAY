using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParentsBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;
    public GameObject previousEnvironment;
    public Transform previousEnvironmentPosition;
    public bool transitioning = true;
    public Transform playerSpawnPosition; 
    public Transform bossSpawnPosition; 
    void Start()
    {
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponentInChildren<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transitioning)
        {
            Vector3 distance = gameObject.transform.position - previousEnvironment.transform.position;
            previousEnvironment.transform.Translate(distance * Time.deltaTime);
            previousEnvironment.transform.localScale -= Time.deltaTime * Vector3.one / 10.0f;
            if (player.GetComponent<PlayerHp>().lifeTime < 15.0f)
            {
                player.transform.position = playerSpawnPosition.position;
                gameObject.transform.position = bossSpawnPosition.position;
            }
            if (previousEnvironment.transform.localScale.x < 0.0f)
            {
                transitioning = false;
                Destroy(previousEnvironment);
                player.GetComponent<Rigidbody>().useGravity = true;
                player.GetComponentInChildren<BoxCollider>().enabled = true;
            }
        }
    }
}
