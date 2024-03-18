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
    public GameObject parentsHouse;
    public Transform dadPosition;
    Vector3 distanceToChair;

    public GameObject houseDoor1;
    public GameObject houseDoor2;
    public Transform houseDoorPosition1;
    public Transform houseDoorPosition2;
    public float doorTimer = 0;
    public bool readyToCombat = false;
    public bool closeDoors = false;

    public GameObject isPlayerAtHome;
    void Start()
    {
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponentInChildren<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        gameObject.GetComponentInChildren<BoxCollider>().isTrigger = true;
        ParticleSystem[] allParticleSystems = FindObjectsOfType<ParticleSystem>();

        // Iterate through each audio source and set its volume
        foreach (ParticleSystem particleSystem in allParticleSystems)
        {
            // Clamp the volume between 0 and 1
            Destroy(particleSystem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<EnemyHP>().hp = 10;
        gameObject.GetComponent<EnemyHP>().canBeDamaged = false;
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
                parentsHouse.SetActive(true);
                transitioning = false;
                Destroy(previousEnvironment);
                player.GetComponent<Rigidbody>().useGravity = true;
                distanceToChair = dadPosition.position - gameObject.transform.position;
                player.GetComponentInChildren<BoxCollider>().enabled = true;
            }
        }
        else if(readyToCombat == false)
        {
            distanceToChair.Normalize();
            if (Vector3.Distance(dadPosition.position,gameObject.transform.position) > 0.35f && doorTimer < 0.01f)
            {
                transform.position += distanceToChair * 7.5f * Time.deltaTime;
                gameObject.transform.LookAt(player.transform.position);
            }
            if (parentsHouse.transform.position.y > -49.5f) parentsHouse.transform.Translate(0, -Time.deltaTime * 10.0f, 0);
            if(Vector3.Distance(dadPosition.position, gameObject.transform.position) <= 7.45f && parentsHouse.transform.position.y <= -48.5f)
            {
                gameObject.transform.position = dadPosition.position;
                doorTimer += Time.deltaTime;
                if (doorTimer < 5.25f)
                {

                    houseDoor1.transform.RotateAround(houseDoorPosition1.transform.position, Vector3.up, Time.deltaTime * 30.0f);
                    houseDoor2.transform.RotateAround(houseDoorPosition2.transform.position, Vector3.up, -Time.deltaTime * 30.0f);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody>().useGravity = true;
                    gameObject.GetComponentInChildren<BoxCollider>().isTrigger = false;
                    gameObject.GetComponentInChildren<BoxCollider>().enabled = true;
                    doorTimer = 0.0f;
                    Debug.Log("House Created Successfully");
                    readyToCombat = true;
                    //Activate Dad Script, Activate Mom, destroy script
                }

            }
        
        }
        if (readyToCombat == true && isPlayerAtHome.GetComponent<isPlayerAtHome>().atHome == true)
        {
            closeDoors = true;
        }
        if(closeDoors == true)
        {
            doorTimer += Time.deltaTime;
            if (doorTimer < 1.05f)
            {
                houseDoor1.transform.RotateAround(houseDoorPosition1.transform.position, Vector3.up, -Time.deltaTime * 150.0f);
                houseDoor2.transform.RotateAround(houseDoorPosition2.transform.position, Vector3.up, Time.deltaTime * 150.0f);
            }
        }
    }
}
