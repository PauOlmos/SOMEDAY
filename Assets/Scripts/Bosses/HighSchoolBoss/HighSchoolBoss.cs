using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class HighSchoolBoss : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject player;

    public bool canMove = true;
    public bool canAttack = false;

    public LayerMask Ground;

    public int phase = 0;
    public int maxPhases = 3;

    public NavMeshAgent agent;
    public GameObject proximityArea;

    public NavMeshSurface floor;

    public float stamina = 0.0f;
    private float bossDistance = 15.5f;

    public enum MovementState
    {
    }
    public enum AttackType
    {
    }
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        phase = 0;
        gameObject.GetComponent<EnemyHP>().hp = 20;
        agent.enabled = true;
        agent.destination = gameObject.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        stamina += Time.deltaTime;
        switch (phase)
        {
            case 0:

                if (canMove)
                {
                    if(stamina > 15.0f)
                    {
                        agent.updateRotation = true;

                        agent.destination = player.transform.position;
                    }
                    else
                    {
                        agent.updateRotation = false;
                        gameObject.transform.LookAt(player.transform.position);
                        Vector3 direccion = transform.position - player.transform.position;
                        float distanciaActual = direccion.magnitude;
                        Vector3 desplazamiento = direccion.normalized * (bossDistance - distanciaActual);

                        // Movemos el objeto en la dirección del desplazamiento
                        agent.destination = desplazamiento;
                    }
                }
                
                break;

            case 1:

                break;

            case 2:

                break;
        }

    }

    public bool IsNear(float distanceToAttack)
    {
        if (Vector3.Distance(player.transform.position, proximityArea.transform.position) < distanceToAttack) { return true; }
        else { return false; }
    }

}