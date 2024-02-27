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
    public float specialAbility = 0.0f;
    private float bossDistance = 25.5f;

    public bool specialAttacking = false;

    public bool attackSelected = false;
    public float attackCooldown = 3.0f;
    public float attackCooldownTimer = 0.0f;

    public bool firstDone = false;
    public bool secondDone = false;

    public GameObject[] tables;
    public GameObject teacherTable;

    public GameObject[] tableAttackPositions;
    public GameObject[] tableRestPositions;

    public GameObject armari1;
    public GameObject armari2;

    public GameObject armariPos1;
    public GameObject armariPos2;
    public enum MovementState
    {
        seeking,hiding,toTable,
    }
    public enum AttackType
    {
        one,two,three,special,reset
    }


    MovementState movState = MovementState.hiding;
    AttackType attackType = AttackType.one;
    AttackType specialAttackPhase = AttackType.one;
    private float stunTimer;

    public bool tablesOnPosition = false;
    public bool inPairRotation = false;
    public bool allTablesReady = false;
    public bool armariRotation = false;
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
        specialAbility += Time.deltaTime;
        switch (phase)
        {
            case 0:

                if (canMove && gameObject.GetComponent<EnemyHP>().stun == false)
                {
                    CollideTables();
                    if(specialAbility > 60.0f)
                    {
                        specialAbility = 0.0f;
                        specialAttacking = true;
                        movState = MovementState.toTable;
                    }
                    attackCooldownTimer += Time.deltaTime;
                    gameObject.GetComponent<EnemyHP>().canBeDamaged = true;
                    if (specialAttacking == false)
                    {
                        if (stamina > 15.0f)
                        {
                            movState = MovementState.seeking;
                        }
                        else
                        {
                            movState = MovementState.hiding;
                        }
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
                        case MovementState.toTable:
                            attackSelected = true;
                            agent.destination = teacherTable.transform.position;
                            if (Vector3.Distance(gameObject.transform.position,teacherTable.transform.position)<1.0f)
                            {
                                canAttack = true;
                                attackType = AttackType.special;
                                canMove = false;
                                specialAttacking = true;
                            }
                            break;
                    }
                    

                    if (IsNear(2.0f) && attackCooldownTimer > attackCooldown && specialAttacking == false)
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
                            case AttackType.special:
                                switch (specialAttackPhase)
                                {
                                    case AttackType.one:
                                        MoveTables();

                                        break;
                                    case AttackType.two:
                                        PushTables();

                                        break;
                                    case AttackType.three:
                                        MoveArmarios();

                                        break;
                                    case AttackType.special:
                                        PushArmarios();

                                        break;
                                    case AttackType.reset:

                                            
                                        break;
                                }
                                break;
                        }
                    }
                }

                if (gameObject.GetComponent<EnemyHP>().stun == true && specialAttacking == false)
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

    private void PushArmarios()
    {
        if(armari1.GetComponent<Rigidbody>() == null)
        {
            armari1.AddComponent<Rigidbody>();
            armari1.GetComponent<Rigidbody>().useGravity = true;
            armari1.tag = "NonParryable";
            armari1.GetComponent<Rigidbody>().freezeRotation = true;
            armari1.GetComponent<Rigidbody>().mass = 100.0f;
            armari1.layer = 7;
        }
        if(armari2.GetComponent<Rigidbody>() == null)
        {
            armari2.tag = "NonParryable";
            armari2.AddComponent<Rigidbody>();
            armari2.GetComponent<Rigidbody>().useGravity = true;
            armari2.GetComponent<Rigidbody>().freezeRotation = true;
            armari2.GetComponent<Rigidbody>().mass = 100.0f;
            armari2.layer = 7;
        }

        if(armari1.transform.position.y < 0.51f || armari2.transform.position.y < 0.51f)
        {
            specialAttackPhase = AttackType.reset;
        }
    }

    private void MoveArmarios()
    {
        Vector3 direction1 = armariPos1.transform.position - armari1.transform.position;
        Vector3 direction2 = armariPos2.transform.position - armari2.transform.position;
        armari1.transform.Translate(direction1 * Time.deltaTime);
        armari2.transform.Translate(direction2 * Time.deltaTime);

        
        if ((direction1).magnitude < 0.1f && (direction1).magnitude < 0.1f)
        {
            if (armari1.transform.eulerAngles.z < 90)
            {
                armari1.transform.Rotate(0, 0, Time.deltaTime * 20);
            }
            else
            {
                armari1.transform.Rotate(0, 0, -(armari1.transform.eulerAngles.z - 90));
                armariRotation = true;
            }
            if (armariRotation == false)
            {
                armari2.transform.Rotate(0, 0, -Time.deltaTime * 20);
            }
            else
            {
                armari2.transform.Rotate(0, 0, 270 - armari2.transform.eulerAngles.z);
                specialAttackPhase = AttackType.special;

            }
        }

    }

    private void PushTables()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            if (tables[i].GetComponent<Rigidbody>() == null)
            {
                tables[i].AddComponent<Rigidbody>();
                tables[i].GetComponent<Rigidbody>().useGravity = false;
                tables[i].GetComponent<Rigidbody>().freezeRotation = true;
                tables[i].GetComponent<Rigidbody>().mass = 100.0f;
                tables[i].layer = 7;
            }
            tables[i].tag = "NonParryable";
            tables[i].transform.Translate(Vector3.up * Time.deltaTime * 5.0f);

            if(tables[i].GetComponent<ParInpar>().par == false)
            {
                if(tables[i].transform.position.x > 10.5)
                {
                    specialAttackPhase = AttackType.three;
                }
            }
            else
            {
                if (tables[i].transform.position.x < -10.5)
                {
                    specialAttackPhase = AttackType.three;
                }
            }

        }
    }

    public void MoveTables()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            Vector3 direction = tableAttackPositions[i].transform.position - tables[i].transform.position;

            if (tablesOnPosition == false)
            {
                tables[i].transform.Translate(direction * Time.deltaTime);
            }

            if (tables[i].GetComponent<ParInpar>().par == true)
            {
                if (tables[i].transform.eulerAngles.z < 90.0f)
                {
                    tables[i].transform.Rotate(0, 0, Time.deltaTime * 15f);
                }
                else
                {
                    inPairRotation = true;
                    tables[i].transform.Rotate(0, 0, -(tables[i].transform.eulerAngles.z - 90));
                }
            }
            else
            {
                if (inPairRotation == false) tables[i].transform.Rotate(0, 0, -Time.deltaTime * 15f);
                else
                {
                    tables[i].transform.Rotate(0, 0, 270 - tables[i].transform.eulerAngles.z);
                    specialAttackPhase = AttackType.two;
                }
            }
            if ((direction).magnitude < 0.1f)
            {
                tablesOnPosition = true;
                Debug.Log("JoinTables");
            }
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

            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), table.GetComponent<BoxCollider>(),false);
            
        }
    }
    public bool AttackThreeTimes()
    {
        proximityAreaTimer += Time.deltaTime;

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