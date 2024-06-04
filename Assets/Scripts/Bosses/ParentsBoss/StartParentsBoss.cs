using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

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
    public Transform momPosition;
    Vector3 distanceToChair;

    public GameObject houseDoor1;
    public GameObject houseDoor2;
    public Transform houseDoorPosition1;
    public Transform houseDoorPosition2;
    public float doorTimer = 0;
    public bool readyToCombat = false;
    public bool closeDoors = false;

    public GameObject isPlayerAtHome;
    public GameObject momBoss;
    public Transform center;
    public NavMeshSurface floor;

    public Transform mainDadProjectileSource;
    public GameObject projectilePrefab;
    public Transform[] bulletHell1Directions;
    public Transform bulletHell2Direction;
    public Material returnableProjectileMaterial;

    public Transform[] bulletHell3Directions;
    public GameObject shotgun;
    public Transform[] rightShotgunPositions;
    public Transform[] leftShotgunPositions;
    public GameObject circularArea;
    public GameObject coneArea;
    public GameObject spikePrefab;
    public GameObject spinner;
    public Transform spinnerArea;
    public GameObject greatAttackArea;

    public GameObject greatAttackArea1;
    public GameObject greatAttackArea2;
    public int difficulty;

    public GameObject highSchoolBossModel;
    public GameObject dadBossModel;

    public AudioSource bossAudioSource;
    public AudioSource momAudioSource;
    public AudioClip[] parentsBossAudios;

    void Start()
    {
        
        player.GetComponent<Rigidbody>().useGravity = false;
        player.GetComponentInChildren<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponentInChildren<BoxCollider>().enabled = false;
        gameObject.GetComponentInChildren<BoxCollider>().isTrigger = true;
        ParticleSystem[] allParticleSystems = FindObjectsOfType<ParticleSystem>();
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        // Iterate through each audio source and set its volume
        foreach (ParticleSystem particleSystem in allParticleSystems)
        {
            // Clamp the volume between 0 and 1
            if(particleSystem != BossManager.swapModelsParticleSystem)Destroy(particleSystem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<EnemyHP>().hp = 30;
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
            if(Vector3.Distance(dadPosition.position, gameObject.transform.position) <= 20.45f && parentsHouse.transform.position.y <= -48.5f)
            {
                gameObject.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                gameObject.transform.position = dadPosition.position;
                Destroy(highSchoolBossModel);
                dadBossModel.SetActive(true); 
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                gameObject.GetComponent<Rigidbody>().mass = 100000;
                momBoss.transform.position = momPosition.position;
                momBoss.SetActive(true);
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
                   // Debug.Log("House Created Successfully");
                    readyToCombat = true;
                }

            }
        
        }
        if (isPlayerAtHome != null)
        {
            if (readyToCombat == true && isPlayerAtHome.GetComponent<isPlayerAtHome>().atHome == true)
            {
                closeDoors = true;
                Destroy(isPlayerAtHome);
            }
        }
        
        if(closeDoors == true)
        {
            doorTimer += Time.deltaTime;
            if (doorTimer < 1.05f)
            {
                houseDoor1.transform.RotateAround(houseDoorPosition1.transform.position, Vector3.up, -Time.deltaTime * 150.0f);
                houseDoor2.transform.RotateAround(houseDoorPosition2.transform.position, Vector3.up, Time.deltaTime * 150.0f);
            }
            else
            {
                //Dad
                gameObject.AddComponent<DadBoss>();
                gameObject.GetComponent<EnemyHP>().canBeDamaged = true;
                gameObject.transform.LookAt(center);
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;

                gameObject.GetComponent<DadBoss>().mainProjectileSource = mainDadProjectileSource;
                gameObject.GetComponent<DadBoss>().bulletHell1Directions = bulletHell1Directions;
                gameObject.GetComponent<DadBoss>().projectilePrefab = projectilePrefab;
                gameObject.GetComponent<DadBoss>().returnableProjectileMaterial = returnableProjectileMaterial;
                gameObject.GetComponent<DadBoss>().bulletHell2Direction = bulletHell2Direction;
                gameObject.GetComponent<DadBoss>().player = player;
                gameObject.GetComponent<DadBoss>().bulletHell3Directions = bulletHell3Directions;
                gameObject.GetComponent<DadBoss>().shotgun = shotgun;
                gameObject.GetComponent<DadBoss>().rightShotgunPositions = rightShotgunPositions;
                gameObject.GetComponent<DadBoss>().leftShotgunPositions = leftShotgunPositions;
                gameObject.GetComponent<DadBoss>().spinner = spinner;
                gameObject.GetComponent<DadBoss>().spinnerArea = spinnerArea;
                gameObject.GetComponent<DadBoss>().difficulty = difficulty;
                gameObject.GetComponent<DadBoss>().dadPosition = dadPosition;
                gameObject.GetComponent<DadBoss>().bossAudioSource = bossAudioSource;
                gameObject.GetComponent<DadBoss>().parentsBossAudios = parentsBossAudios;

                //Mom
                momBoss.AddComponent<MomBoss>();
                floor.BuildNavMesh();

                momBoss.GetComponent<MomBoss>().player = player;
                momBoss.GetComponent<MomBoss>().circularArea = circularArea;
                momBoss.GetComponent<MomBoss>().coneArea = coneArea;
                momBoss.GetComponent<MomBoss>().spikePrefab = spikePrefab;
                momBoss.GetComponent<MomBoss>().greatAttackArea = greatAttackArea;
                momBoss.GetComponent<MomBoss>().greatAttackArea1 = greatAttackArea1;
                momBoss.GetComponent<MomBoss>().greatAttackArea2 = greatAttackArea2;
                momBoss.GetComponent<MomBoss>().difficulty = difficulty;
                momBoss.GetComponent<MomBoss>().momAudioSource = momAudioSource;
                momBoss.GetComponent<MomBoss>().parentsBossAudios = parentsBossAudios;


                Destroy(gameObject.GetComponent<StartParentsBoss>());
            }
        }
    }
}
