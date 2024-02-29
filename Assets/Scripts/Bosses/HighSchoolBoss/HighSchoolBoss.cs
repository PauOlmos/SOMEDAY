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
    public float attackCooldown = 3.5f;
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

    public GameObject armariResetPos1;
    public GameObject armariResetPos2;

    public GameObject bossShield;
    public GameObject portalSpawnArea;

    public GameObject portalPrefab;
    public bool touchingGround = false;
    public enum MovementState
    {
        seeking,hiding,toTable,
    }
    public enum AttackType
    {
        one,two,three,special,reset,portals
    }


    MovementState movState = MovementState.hiding;
    AttackType attackType = AttackType.one;
    AttackType specialAttackPhase = AttackType.one;
    private float stunTimer;

    public bool tablesOnPosition = false;
    public bool inPairRotation = false;
    public bool allTablesReady = false;
    public bool armariRotation = false;

    public int hitsToSuperAttack = 3;
    public float superAttackTimer = 0.0f;
    public bool portalWave1 = false;
    public bool portalWave2 = false;
    public bool portalWave3 = false;
    public bool portalWave4 = false;

    public bool transitionToCorridor = false;
    public bool openDoors = false;

    public GameObject corridorPos;
    public GameObject wall4;
    public GameObject door1;
    public GameObject door2;
    public GameObject doorPos1;
    public GameObject doorPos2;
    public GameObject projectileSource;
    public GameObject projectilePrefab;
    public bool teleportBoss = false;
    public float doorTimer = 0.0f;
    public float floorMultiplier = 19.0f;
    public float corridorAttackCooldownTimer = 0.0f;

    public float projectileAttackTimer = 0.0f;
    public int numProjectilesShot = 0;


    public GameObject hand1;
    public GameObject hand2;
    public GameObject hand3;
    public GameObject handDamage1;
    public GameObject handDamage2;
    public GameObject handDamage3;
    public GameObject handPos1;
    public GameObject handPos2;
    public GameObject handPos3;

    public GameObject weakPoint1;
    public GameObject weakPoint2;
    public GameObject weakPoint3;

    public bool rotatingHands = false;

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        phase = 0;
        gameObject.GetComponent<EnemyHP>().hp = 100;
        agent.enabled = true;
        agent.destination = gameObject.transform.position;
        armariResetPos1.transform.position = armari1.transform.position;
        armariResetPos2.transform.position = armari2.transform.position;
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
                            agent.speed = 3.5f;

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
                            agent.speed = 3.5f;

                            agent.updateRotation = true;
                            Vector3 direction = transform.position - player.transform.position;
                            float distanceActual = direction.magnitude;
                            Debug.Log("dISTANCIAaCTUAL = " + distanceActual);

                            if (distanceActual > 2.0f) agent.destination = player.transform.position;
                            else agent.destination = gameObject.transform.position;
                            break;
                        case MovementState.toTable:
                            gameObject.GetComponent<EnemyHP>().canBeDamaged = false;
                            attackSelected = true;
                            agent.destination = teacherTable.transform.position;
                            agent.speed = 20.0f;
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
                                        int numReset = 0;
                                        numReset += ResetTablesPositions();
                                        numReset += ResetTablesRotations();
                                        numReset += ResetArmarisPositions();
                                        numReset += ResetArmarisRotations();
                                            
                                        if(numReset == 4)
                                        {
                                            ResetSpecialAttackVariables();
                                        }
                                        break;
                                }
                                break;
                            case AttackType.portals:
                                superAttackTimer += Time.deltaTime;
                                agent.destination = gameObject.transform.position;

                                gameObject.GetComponent<EnemyHP>().canBeDamaged = false;

                                bossShield.SetActive(true);

                                if (bossShield.transform.localScale.x > 2.0f) bossShield.transform.localScale = Vector3.one * 2.0f;
                                else bossShield.transform.localScale += Vector3.one * Time.deltaTime;

                                if(portalWave1 == false) GenerarPosiciones(3, portalSpawnArea.transform, 1);
                                if(portalWave2 == false && superAttackTimer > 1.5f) GenerarPosiciones(5, portalSpawnArea.transform, 2);
                                if(portalWave3 == false && superAttackTimer > 3.0f) GenerarPosiciones(15, portalSpawnArea.transform, 3);
                                if(portalWave4 == false && superAttackTimer > 4.5f) GenerarPosiciones(15, portalSpawnArea.transform, 4);
                                if(superAttackTimer > 6.5f)
                                {
                                    ResetSuperAttackVariables();
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

                if(transitionToCorridor == false)
                {
                    EveryoneToCorrdor();
                }
                else
                {
                    if(touchingGround == true)
                    {
                        player.GetComponent<Rigidbody>().velocity -= new Vector3(0, 0, floorMultiplier * Time.deltaTime);
                    }
                    if (canAttack)
                    {
                        switch (attackType)
                        {
                            case AttackType.one:
                                projectileAttackTimer += Time.deltaTime;

                                if(projectileAttackTimer > 0.25f)
                                {
                                    GameObject projectile = Instantiate(projectilePrefab, projectileSource.transform.position, Quaternion.identity);
                                    projectile.AddComponent<SeekingProjectile>();
                                    projectile.GetComponent<SeekingProjectile>().canFail = true;
                                    projectile.GetComponent<SeekingProjectile>().shotByPlayer = false;
                                    projectile.GetComponent<SeekingProjectile>().seekingTime = 0.01f;
                                    projectile.GetComponent<SeekingProjectile>().target = player.transform;
                                    projectile.GetComponent<SeekingProjectile>().speed = 25.0f;
                                    projectile.tag = "BasicProjectile";
                                    projectile.layer = 7;
                                    projectile.AddComponent<DieByTime>();
                                    projectile.GetComponent<DieByTime>().deathTime = 10.0f;
                                    numProjectilesShot++;
                                    projectileAttackTimer = 0.0f;
                                }

                                if (numProjectilesShot > 4)
                                {
                                    canAttack = false;
                                    attackType = AttackType.reset;
                                }
                                //Dispara

                                break;
                            case AttackType.two:

                                if (hand1.transform.eulerAngles.z - 360 <= 90.0f && rotatingHands == false)
                                {
                                    hand1.transform.RotateAround(handPos1.transform.position, Vector3.forward, -Time.deltaTime * 30.0f);
                                }
                                if (hand2.transform.eulerAngles.z - 360 <= 90.0f && rotatingHands == false)
                                {
                                    hand2.transform.RotateAround(handPos2.transform.position, Vector3.forward, Time.deltaTime * 30.0f);
                                }
                                if (hand3.transform.eulerAngles.z - 360 <= 90.0f && rotatingHands == false)
                                {
                                    hand3.transform.RotateAround(handPos3.transform.position, Vector3.forward, Time.deltaTime * 30.0f);
                                }

                                if(hand1.transform.eulerAngles.z < 271 && hand1.transform.eulerAngles.z > 269)
                                {
                                    rotatingHands = true;
                                    if (handDamage1.GetComponent<Rigidbody>() != null) Destroy(handDamage2.GetComponent<Rigidbody>());
                                }

                                //Braços

                                break;
                        }
                    }
                    else if(attackType == AttackType.reset)
                    {
                        corridorAttackCooldownTimer += Time.deltaTime;
                        if(corridorAttackCooldownTimer > 4.0f)
                        {
                            SelectCorridorAttack();
                        }
                    }
                }

                break;

            case 2:

                break;
        }

    }

    private void SelectCorridorAttack()
    {
        int attack = Random.Range(0, 2);
        switch (attack)
        {
            case 0:
                attackType = AttackType.one;
                numProjectilesShot = 0;
                break;
            case 1:
                attackType = AttackType.two;
                handDamage1.AddComponent<Rigidbody>();
                handDamage1.GetComponent<Rigidbody>().useGravity = false;

                handDamage2.AddComponent<Rigidbody>();
                handDamage2.GetComponent<Rigidbody>().useGravity = false;

                handDamage3.AddComponent<Rigidbody>();
                handDamage3.GetComponent<Rigidbody>().useGravity = false;

                weakPoint1.SetActive(true);

                if(weakPoint1.GetComponent<Rigidbody>() == null) weakPoint1.AddComponent<Rigidbody>();
                weakPoint1.GetComponent<Rigidbody>().useGravity = false;

                weakPoint2.SetActive(true);

                if (weakPoint2.GetComponent<Rigidbody>() == null) weakPoint2.AddComponent<Rigidbody>();
                weakPoint2.GetComponent<Rigidbody>().useGravity = false;

                weakPoint3.SetActive(true);

                if (weakPoint3.GetComponent<Rigidbody>() == null) weakPoint3.AddComponent<Rigidbody>();
                weakPoint3.GetComponent<Rigidbody>().useGravity = false;
                rotatingHands = false;
                break;
        }
        attackType = AttackType.two;

        corridorAttackCooldownTimer = 0.0f;
        canAttack = true;
    }

    private void EveryoneToCorrdor()
    {
        agent.enabled = false;
        Vector3 direction = corridorPos.transform.position - gameObject.transform.position;
        gameObject.GetComponent<EnemyHP>().canBeDamaged = false;
        canMove = false;
        canAttack = false;
        if(teleportBoss == false)
        {
            gameObject.transform.localScale -= Vector3.one * Time.deltaTime;
            if(gameObject.transform.localScale.x <= 0)
            {
                teleportBoss = true;
                gameObject.transform.position = corridorPos.transform.position;
                gameObject.transform.localScale = Vector3.one * 3;
            }
        }
        else gameObject.transform.position = corridorPos.transform.position;
        if (wall4.transform.position.x < 11.5f)
        {
            wall4.transform.Translate(Time.deltaTime * 2.0f, 0, 0);
        }
        wall4.tag = "NonParryable";
        wall4.layer = 7;
        if (direction.magnitude < 0.1f && openDoors == false)
        {
            doorTimer += Time.deltaTime;
            if(doorTimer < 4.0f)
            {
                if (door1.transform.eulerAngles.y - 360 <= 90.0f)
                {
                    door1.transform.RotateAround(doorPos1.transform.position, Vector3.up, Time.deltaTime * 30.0f);
                }
                if (door2.transform.eulerAngles.y - 360 <= 90.0f)
                {
                    door2.transform.RotateAround(doorPos2.transform.position, Vector3.up, Time.deltaTime * 30.0f);
                }
            }
            
            
        }
        //Abrir puertas i checkear si el player esta en el pasillo
        //Cerrar Puertas
        if (player.transform.position.x > 13.0f && wall4.transform.position.x >= 11.5f)
        {
            wall4.tag = "Untagged";
            wall4.layer = 3;
            transitionToCorridor = true;
            attackType = AttackType.reset;
        }
         

    }

    private void ResetSuperAttackVariables()
    {
        portalWave1 = false;
        portalWave2 = false;
        portalWave3 = false;
        portalWave4 = false;
        attackType = AttackType.one;
        canMove = true;
        attackSelected = false;
        canAttack = false;
        superAttackTimer = 0.0f;
        agent.destination = gameObject.transform.position;
        movState = MovementState.hiding;
        bossShield.SetActive(false);
        bossShield.transform.localScale = Vector3.zero;
        gameObject.GetComponent<EnemyHP>().canBeDamaged = true;

    }

    void GenerarPosiciones(int numPositions, Transform area, int portalWave)
    {
        for (int i = 0; i < numPositions; i++)
        {
            // Generar posiciones aleatorias dentro del volumen
            Vector3 posicion = new Vector3(Random.Range(-area.transform.localScale.x / 2, area.transform.localScale.x / 2),
                                           Random.Range(-area.transform.localScale.y / 2, area.transform.localScale.y / 2),
                                           Random.Range(-area.transform.localScale.z / 2, area.transform.localScale.z / 2));

            // Ajustar la posición al espacio del volumen
            posicion += area.transform.position;

            GameObject portal = Instantiate(portalPrefab, posicion, Quaternion.identity);
            portal.GetComponent<Portal>().player = player.transform;
            portal.GetComponent<Portal>().projectileType = portalWave-1;

        }
        switch (portalWave)
        {
            case 1:
                portalWave1 = true;
                    break;
            case 2:
                portalWave2 = true;
                    break;
            case 3:
                portalWave3 = true;
                    break;
            case 4:
                portalWave4 = true;
                    break;
        }
    }

    private void ResetSpecialAttackVariables()
    {
        specialAttackPhase = AttackType.one;
        tablesOnPosition = false;
        inPairRotation = false;
        attackSelected = false;
        specialAttacking = false;
        specialAbility = 0.0f;
        agent.destination = player.transform.position;
        movState = MovementState.seeking;
        attackCooldownTimer = 0.0f;
        gameObject.GetComponent<EnemyHP>().canBeDamaged = true;
        canMove = true;
        canAttack = false;
        firstDone = false;
        secondDone = false;
    }

    private int ResetArmarisRotations()
    {
        armari1.transform.Rotate(-armari1.transform.rotation.eulerAngles);
        armari2.transform.Rotate(-armari2.transform.rotation.eulerAngles);
        return 1;

    }

    private int ResetArmarisPositions()
    {
        armari1.transform.position = armariResetPos1.transform.position;
        armari2.transform.position = armariResetPos2.transform.position;

        if(armari1.GetComponent<Rigidbody>() != null)
        {
            Destroy(armari1.GetComponent<Rigidbody>());
            armari1.tag = "Untagged";
            armari1.layer = 6;
        }
        if(armari2.GetComponent<Rigidbody>() != null)
        {
            Destroy(armari2.GetComponent<Rigidbody>());
            armari2.tag = "Untagged";
            armari2.layer = 6;
        }

        return 1;
    }

    private int ResetTablesRotations()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            tables[i].transform.Rotate(-tables[i].transform.rotation.eulerAngles);

        }
        return 1;

    }

    private int ResetTablesPositions()
    {
        for (int i = 0; i < tables.Length; i++)
        {

            if(tables[i].GetComponent<Rigidbody>() != null)
            {
                Destroy(tables[i].GetComponent<Rigidbody>());
                tables[i].tag = "Table";
                tables[i].layer = 6;

            }
            tables[i].transform.position = tableRestPositions[i].transform.position;
        }
        return 1;
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
        armari1.transform.Translate(direction1 * Time.deltaTime * 5.0f);
        armari2.transform.Translate(direction2 * Time.deltaTime * 5.0f);

        
        if ((direction1).magnitude < 0.1f && (direction1).magnitude < 0.1f)
        {
            if (armari1.transform.eulerAngles.z < 90)
            {
                armari1.transform.Rotate(0, 0, Time.deltaTime * 60);
            }
            else
            {
                armari1.transform.Rotate(0, 0, -(armari1.transform.eulerAngles.z - 90));
                armariRotation = true;
            }
            if (armariRotation == false)
            {
                armari2.transform.Rotate(0, 0, -Time.deltaTime * 60);
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
            tables[i].transform.Translate(Vector3.up * Time.deltaTime * 10.0f);

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
                tables[i].transform.Translate(direction * Time.deltaTime * 4);
            }

            if (tables[i].GetComponent<ParInpar>().par == true)
            {
                if (tables[i].transform.eulerAngles.z < 90.0f)
                {
                    tables[i].transform.Rotate(0, 0, Time.deltaTime * 30f);
                }
                else
                {
                    inPairRotation = true;
                    tables[i].transform.Rotate(0, 0, -(tables[i].transform.eulerAngles.z - 90));
                }
            }
            else
            {
                if (inPairRotation == false) tables[i].transform.Rotate(0, 0, -Time.deltaTime * 30f);
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
        if(hitsToSuperAttack <= 0)
        {
            hitsToSuperAttack += 3;
            attackType = AttackType.portals;
            canMove = false;
            canAttack = true;
        }
        attackSelected = true;
    }

    public bool IsNear(float distanceToAttack)
    {
        if (Vector3.Distance(player.transform.position, proximityArea.transform.position) < distanceToAttack) { return true; }
        else { return false; }
    }

}