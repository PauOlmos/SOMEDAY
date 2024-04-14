using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class StartBrotherBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;

    public GameObject houseDoor1;
    public Transform houseDoorPosition1;
    public Transform houseDoorPosition2;
    public GameObject houseDoor2;
    public float doorTimer = 0.0f;

    public Transform[] bossTrail;
    public float bossTrailingSpeed = 5.0f;
    public int actualSeekingPosition = 0;

    public IsPlayerOnStreet isPlayerOnStreet;
    public GameObject ageCorridor;

    public GameObject car;
    public Transform carDriversPosition;
    public GameObject streets;
    public NavMeshSurface streetsFloor;

    public Transform[] carTrail;
    public float startBossFightTimer = 0.0f;

    public Transform clone1Position;
    public Transform clone2Position;

    public GameObject clone;
    public GameObject proximityAreaAttack;
    public GameObject brotherBossModel;
    public GameObject cloneWall;

    public Transform[] randomMapPositions;
    public enum TransitionState
    {
        start, moving, waiting, driving, startBossFight 
    }

    public TransitionState transitionState = TransitionState.start;

    void Start()
    {
        streets.SetActive(true);
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        brotherBossModel.transform.localPosition = Vector3.zero;

        switch (transitionState)
        {
            case TransitionState.start:

                doorTimer += Time.deltaTime;
                if (doorTimer < 5.25f)
                {
                    houseDoor1.transform.RotateAround(houseDoorPosition1.transform.position, Vector3.up, -Time.deltaTime * 30.0f);
                    houseDoor2.transform.RotateAround(houseDoorPosition2.transform.position, Vector3.up, Time.deltaTime * 30.0f);
                }
                else transitionState = TransitionState.moving;

                break;

            case TransitionState.moving:
                if (actualSeekingPosition <= bossTrail.Length) {
                    if (actualSeekingPosition == bossTrail.Length)
                    {
                        transitionState = TransitionState.waiting;
                        break;
                    }
                    if (Vector3.Distance(gameObject.transform.position, bossTrail[actualSeekingPosition].position) < 1.5f)
                    {
                        actualSeekingPosition++;
                        
                    }
                    else
                    {
                        Vector3 distanceToPoint = bossTrail[actualSeekingPosition].position - gameObject.transform.position;
                        transform.position += distanceToPoint * bossTrailingSpeed * Time.deltaTime;
                        if (actualSeekingPosition < bossTrail.Length - 1) gameObject.transform.LookAt(bossTrail[actualSeekingPosition + 1].position);
                        else gameObject.transform.LookAt(player.transform.position);
                    }
                }
                

                break;

            case TransitionState.waiting:
                gameObject.transform.position = carDriversPosition.position;
                if(isPlayerOnStreet.isOnStreet == true)
                {
                    Destroy(isPlayerOnStreet);
                    transitionState = TransitionState.driving;
                    actualSeekingPosition = 0;
                    streetsFloor.BuildNavMesh();
                }

                break;

            case TransitionState.driving:

                gameObject.transform.position = carDriversPosition.position;
                if (actualSeekingPosition <= carTrail.Length)
                {
                    if (actualSeekingPosition == carTrail.Length)
                    {
                        transitionState = TransitionState.startBossFight;
                        break;
                    }
                    if (Vector3.Distance(car.transform.position, carTrail[actualSeekingPosition].position) < 2.5f && actualSeekingPosition < carTrail.Length - 1)
                    {
                        actualSeekingPosition++;
                    }
                    else
                    {
                        car.GetComponent<NavMeshAgent>().destination = carTrail[actualSeekingPosition].position;
                    }
                    if (Vector3.Distance(car.transform.position, carTrail[actualSeekingPosition].position) < 0.5f && actualSeekingPosition == carTrail.Length - 1)
                    {
                        actualSeekingPosition++;
                    }
                }
                break;
            case TransitionState.startBossFight:

                startBossFightTimer += Time.deltaTime;

                if(startBossFightTimer > 5.0f)//Time for the audio transition (Ex: You killed me brother!!!)
                {
                    //Start BossFight
                    gameObject.GetComponent<EnemyHP>().hp = 40;
                    gameObject.GetComponent<EnemyHP>().canBeDamaged = false;
                    gameObject.AddComponent<BrotherBoss>();
                    gameObject.GetComponent<BrotherBoss>().player = player;
                    gameObject.GetComponent<BrotherBoss>().streets = streets.transform;
                    gameObject.GetComponent<BrotherBoss>().clone = clone;
                    gameObject.GetComponent<BrotherBoss>().cloneWall = cloneWall;
                    gameObject.GetComponent<BrotherBoss>().clone1Position = clone1Position;
                    gameObject.GetComponent<BrotherBoss>().clone2Position = clone2Position;
                    gameObject.GetComponent<BrotherBoss>().proximityAreaAttack = proximityAreaAttack;
                    gameObject.GetComponent<BrotherBoss>().brotherBossModel = brotherBossModel;
                    gameObject.GetComponent<BrotherBoss>().randomMapPositions = randomMapPositions;
                    Destroy(gameObject.GetComponent<StartBrotherBoss>());
                }

                break;
        }


    }
}
