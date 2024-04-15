using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public float timer = 0.0f;
    public float globalTimer = 0.0f;
    public float speed = 10;
    public bool touchingWorld = false;
    public BrotherBoss bBoss;
    public GameObject[] carWheels;
    public enum MovementState
    {
        seekingPlayer, waiting, dashing
    }

    public MovementState mState = MovementState.seekingPlayer;

    public Vector3 direction;
    void Start()
    {
        gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        globalTimer += Time.deltaTime;
       for (int i = 0; i < 4; i++)
       {
            carWheels[i].transform.localEulerAngles += Vector3.right * Time.deltaTime * 10;
       }
        switch (mState)
        {

            case MovementState.seekingPlayer:

                gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;

                if (timer > 5.0f)
                {
                    timer = 0.0f;
                    mState = MovementState.waiting;
                    gameObject.GetComponent<NavMeshAgent>().speed = 0; 
                    direction = (player.transform.position - gameObject.transform.position);
                    gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position + direction.normalized * 1.2f * direction.magnitude;
                }
                break;

            case MovementState.waiting:

                if (timer > 0.5f)
                {
                    gameObject.GetComponent<NavMeshAgent>().speed = speed * 3;
                    mState = MovementState.dashing;
                    timer = 0.0f;
                }
                break;
            case MovementState.dashing:

                if (timer > 5.0f || touchingWorld == true || Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<NavMeshAgent>().destination) < 3.0f)
                {
                    timer = 0.0f;
                    gameObject.GetComponent<NavMeshAgent>().speed = speed;
                    mState = MovementState.seekingPlayer;
                }
                break;

        }

        if(globalTimer > 20.0f)
        {
            for (int i = 0; i < 4; i++)
            {
                carWheels[i].GetComponent<CarWheel>().enabled = false;
            }
            timer = 0.0f;
            globalTimer = 0.0f;
            gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
            gameObject.GetComponent<NavMeshAgent>().speed = speed;
            mState = MovementState.seekingPlayer;
            touchingWorld = false;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            direction = Vector3.zero;
            gameObject.GetComponent<CarAttack>().enabled = false;
            bBoss.canAttack = false;
            bBoss.canMove = true;
            gameObject.tag = "Untagged";
            gameObject.layer = 0;
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7) touchingWorld = true;
    }
    
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 7) touchingWorld = false;
    }

}
