using System;
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
    private float proximityAreaTimer;
    public bool canAttack = false;

    public LayerMask Ground;

    public int phase = 0;
    public int maxPhases = 3;

    public NavMeshAgent agent;
    public GameObject proximityArea;

    public NavMeshSurface floor;

    public float stamina = 0.0f;
    private float bossDistance = 25.5f;

    public bool attackSelected = false;
    public float attackCooldown = 3.0f;
    public float attackCooldownTimer = 0.0f;

    public bool firstDone = false;
    public bool secondDone = false;

    public GameObject[] tables;
    public enum MovementState
    {
        seeking,hiding
    }
    public enum AttackType
    {
        one,two,three,
    }

    MovementState movState = MovementState.hiding;
    AttackType attackType = AttackType.one;
    private float stunTimer;

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

                if (canMove && gameObject.GetComponent<EnemyHP>().stun == false)
                {
                    CollideTables();

                    attackCooldownTimer += Time.deltaTime;
                    gameObject.GetComponent<EnemyHP>().canBeDamaged = true;
                    if (stamina > 15.0f)
                    {
                        movState = MovementState.seeking;
                    }
                    else
                    {
                        movState = MovementState.hiding;
                    }
                    switch (movState)
                    {
                        case MovementState.hiding:
                            agent.updateRotation = false;
                            gameObject.transform.LookAt(player.transform.position);
                            Vector3 direccion = transform.position - player.transform.position;
                            float distanciaActual = direccion.magnitude;
                            Vector3 desplazamiento = direccion.normalized * (bossDistance - distanciaActual);
                            // Movemos el objeto en la dirección del desplazamiento
                            if (distanciaActual > 5.5f) agent.destination = desplazamiento;
                            else agent.destination = gameObject.transform.position;
                            break;
                        case MovementState.seeking:
                            agent.updateRotation = true;
                            Vector3 direction = transform.position - player.transform.position;
                            float distanceActual = direction.magnitude;
                            Debug.Log("dISTANCIAaCTUAL = " + distanceActual);

                            if (distanceActual > 2.0f) agent.destination = player.transform.position;
                            else agent.destination = gameObject.transform.position;
                            break;
                    }
                    

                    if (IsNear(2.0f) && attackCooldownTimer > attackCooldown)
                    {
                        canAttack = true;
                        firstDone = false;
                        secondDone = false;

                    }

                }
                if (canAttack && gameObject.GetComponent<EnemyHP>().stun == false)
                {
                    if(attackSelected == false)
                    {
                        SelectAttack(phase);
                    }
                    else
                    {
                        switch (attackType)
                        {
                            case AttackType.one:
                                IgnoreTables();
                                AttackOnce();
                                
                                break;
                            case AttackType.two:
                                IgnoreTables();
                                AttackTwice();
                                
                                break;
                            case AttackType.three:
                                IgnoreTables();
                                AttackThreeTimes();
                                break;
                        }
                    }
                }

                if (gameObject.GetComponent<EnemyHP>().stun == true)
                {
                    stunTimer += Time.deltaTime;
                    if (stunTimer > 3.0f)
                    {
                        stunTimer = 0.0f;
                        gameObject.GetComponent<EnemyHP>().stun = false;
                        canMove = true;
                    }
                    else
                    {
                        attackSelected = false;
                        proximityAreaTimer = 0.0f;
                        canAttack = false;
                        canMove = false;
                    }
                }
                break;

            case 1:

                break;

            case 2:

                break;
        }

    }

    public void IgnoreTables()
    {
        foreach (GameObject table in tables)
        {

            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), table.GetComponent<Collider>());
            
        }
    }
    public void CollideTables()
    {
        foreach (GameObject table in tables)
        {

            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), table.GetComponent<Collider>(),false);
            
        }
    }
    public bool AttackThreeTimes()
    {
        proximityAreaTimer += Time.deltaTime;
        //gameObject.transform.LookAt(player.transform.position);

        if (proximityAreaTimer > 0.2f && proximityArea.activeInHierarchy == false && firstDone == false)
        {
            proximityArea.SetActive(true);
            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            proximityArea.tag = "Parryable";
            agent.destination = player.transform.position;

            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 40.0f, ForceMode.VelocityChange);
        }
        if (proximityAreaTimer > 0.5f && proximityArea.activeInHierarchy == true && firstDone == false)
        {
            proximityArea.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            proximityArea.tag = "Parryable";
        }
        if (proximityAreaTimer > 1.0f && firstDone == false)
        {
            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            proximityArea.SetActive(false);
            proximityArea.tag = "Parryable";
            firstDone = true;
        }
        if (proximityAreaTimer > 1.5f && proximityArea.activeInHierarchy == false && secondDone == false)
        {
            proximityArea.SetActive(true);
            proximityArea.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            proximityArea.tag = "Parryable";
            agent.destination = player.transform.position;

            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 30.0f, ForceMode.VelocityChange);
        }
        if (proximityAreaTimer > 1.8f && proximityArea.activeInHierarchy == true && secondDone == false)
        {
            proximityArea.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
            proximityArea.tag = "Parryable";
        }
        if (proximityAreaTimer > 2.3f && secondDone == false)
        {
            agent.destination = gameObject.transform.position;

            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            proximityArea.SetActive(false);
            proximityArea.tag = "Parryable";
            secondDone = true;
        }
        if (proximityAreaTimer > 3.2f && proximityArea.activeInHierarchy == false)
        {
            proximityArea.SetActive(true);
            proximityArea.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
            proximityArea.tag = "NonParryable";
        }
        if (proximityAreaTimer > 3.9f)
        {
            proximityArea.tag = "Parryable";
            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

            proximityArea.SetActive(false);
            attackCooldownTimer = 0.0f;
            firstDone = false;
            secondDone = false;
            canMove = true;
            stamina = 0.0f;
            attackSelected = false;
            proximityAreaTimer = 0.0f;
            canAttack = false;
            return true;
        }
        canMove = false;
        return false;
    }
    private bool AttackTwice()
    {
        proximityAreaTimer += Time.deltaTime;
        //gameObject.transform.LookAt(player.transform.position);

        if (proximityAreaTimer > 0.2f && proximityArea.activeInHierarchy == false && firstDone == false)
        {
            proximityArea.SetActive(true);
            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            proximityArea.tag = "Parryable";
            agent.destination = player.transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 40.0f, ForceMode.VelocityChange);
        }
        if (proximityAreaTimer > 0.5f && proximityArea.activeInHierarchy == true && firstDone == false)
        {
            proximityArea.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            proximityArea.tag = "Parryable";
        }
        if (proximityAreaTimer > 1.0f && firstDone == false)
        {
            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            proximityArea.SetActive(false);
            proximityArea.tag = "Parryable";
            firstDone = true;
        }
        if (proximityAreaTimer > 1.5f && proximityArea.activeInHierarchy == false)
        {
            proximityArea.SetActive(true);
            proximityArea.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            proximityArea.tag = "Parryable";
            agent.destination = player.transform.position;
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 30.0f, ForceMode.VelocityChange);
        }
        if (proximityAreaTimer > 1.8f && proximityArea.activeInHierarchy == true)
        {
            proximityArea.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
            proximityArea.tag = "Parryable";
        }
        if (proximityAreaTimer > 2.3f)
        {
            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            proximityArea.SetActive(false);
            proximityArea.tag = "Parryable";
            attackCooldownTimer = 0.0f;
            firstDone = false;
            canMove = true;
            stamina = 0.0f;
            attackSelected = false;
            proximityAreaTimer = 0.0f;
            canAttack = false;
            return true;
        }

        canMove = false;
        return false;
    }

    public bool AttackOnce()
    {
        proximityAreaTimer += Time.deltaTime;

        if (proximityAreaTimer > 0.2f && proximityArea.activeInHierarchy == false)
        {
            proximityArea.SetActive(true);
            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            proximityArea.tag = "Parryable";
            agent.destination = gameObject.transform.forward * 2;
            gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 40.0f, ForceMode.VelocityChange);
        }
        if (proximityAreaTimer > 0.5f && proximityArea.activeInHierarchy == true)
        {
            proximityArea.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            proximityArea.tag = "Parryable";
        }
        if (proximityAreaTimer > 1.0f)
        {
            proximityArea.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            proximityArea.SetActive(false);
            proximityArea.tag = "Parryable";
            attackCooldownTimer = 0.0f;
            canMove = true;
            stamina = 0.0f;
            attackSelected = false;
            proximityAreaTimer = 0.0f;
            canAttack = false;
            return true;
        }
        canMove = false;

        return false;
    }

    private void SelectAttack(int phase)
    {
        switch (phase)
        {
            case 0:

                if(stamina < 15.0f)
                {
                    attackType = AttackType.one;
                }
                else if(stamina < 20.0f)
                {
                    attackType = AttackType.two;
                }
                else
                {
                    attackType = AttackType.three;
                }
                attackType = AttackType.three;

                break;
        }
        attackSelected = true;
    }

    public bool IsNear(float distanceToAttack)
    {
        if (Vector3.Distance(player.transform.position, proximityArea.transform.position) < distanceToAttack) { return true; }
        else { return false; }
    }

}