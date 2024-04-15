using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BrotherBoss : MonoBehaviour
{

    public int phase = 0;
    public float stunTimer;
    public float speed = 5.0f;
    public float acceleration = 8.0f;

    public enum AttackType
    {
        trio, circle, walls, disc, car, drones, fall
    }

    public enum MovementState
    {
        seeking, hiding, aerial,
    }
    public enum FallAttackState
    {
        positioning, falling, resting, protecting
    }

    public FallAttackState fallState = FallAttackState.positioning;
    public MovementState mState = MovementState.seeking;

    public AttackType attackType;

    public bool attackSelected = false;

    public GameObject player;
    public GameObject proximityAreaAttack;

    public Transform clone1Position;
    public Transform clone2Position;

    public GameObject clone;
    public GameObject cloneWall;

    public float attack1Timer = 0.0f;
    public float attack1Cooldown = 0.6f;

    public float vulnerabilityTimer = 0.0f;

    public float cooldownTimer = 0.0f;
    public float attackCooldown = 2.0f;

    public bool canAttack = false;
    public bool canMove = true;

    public bool bossDash = false;
    public Vector3 dashDirection;
    public float dashSpeed = 50.0f;
    public float dashAcceleration = 50.0f;
    public Transform streets;
    public GameObject brotherBossModel;

    public Transform[] randomMapPositions;
    public float getPositionTimer = 0.0f;

    public float cloneRadius = 20.0f;
    public float passiveCharge = 0.0f;
    public bool activeWalls = false;

    public Transform aerialPosition;
    public Transform[] discMovementArea;
    public GameObject disc;

    public int[] dronesCooldown = { 0, 1, 2 };
    public int numDrones = 0;
    public float droneTimer = 0.0f;
    public GameObject drone;

    public GameObject car;
    public float fallTimer = 0.0f;
    public bool landed = false;
    public GameObject continousCircle;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().speed = speed;
        gameObject.GetComponent<NavMeshAgent>().angularSpeed = 300;
        proximityAreaAttack.transform.position = Vector3.zero;
        proximityAreaAttack.transform.localScale = Vector3.one * 2;
        proximityAreaAttack.transform.SetParent(brotherBossModel.transform);
        if (gameObject.GetComponent<CapsuleCollider>() == null) gameObject.AddComponent<CapsuleCollider>();
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        proximityAreaAttack.transform.localPosition = Vector3.zero;
        brotherBossModel.transform.localPosition = Vector3.zero;

        switch (phase)
        {
            case 0:

                passiveCharge += Time.deltaTime;

                if (canMove)
                {
                    switch (mState)
                    {
                        case MovementState.seeking:

                            gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
                            cooldownTimer += Time.deltaTime;
                            if (cooldownTimer > attackCooldown && attackSelected == false)
                            {
                                SelectAttack();
                            }
                            else if (attackSelected == true)
                            {
                                if (attackType == AttackType.trio && Vector3.Distance(gameObject.transform.position, player.transform.position) < 25.0f)
                                {
                                    canMove = false;
                                    canAttack = true;
                                }
                                else if (attackType == AttackType.trio)
                                {
                                    gameObject.GetComponent<NavMeshAgent>().speed = speed;
                                }
                                if (attackType == AttackType.circle)
                                {
                                    canMove = false;
                                    canAttack = true;
                                }
                                if (attackType == AttackType.walls)
                                {
                                    canMove = false;
                                    canAttack = true;
                                }
                            }

                            break;
                        case MovementState.hiding:

                            if(activeWalls == true && passiveCharge < 30)
                            {
                                gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
                                gameObject.GetComponent<NavMeshAgent>().speed = speed / 2;
                                cooldownTimer += Time.deltaTime;
                                if(cooldownTimer > 5.0f)
                                {
                                    mState = MovementState.seeking;
                                    cooldownTimer = 0.0f;
                                }
                            }
                            else
                            {
                                if (passiveCharge >= 30) activeWalls = false;
                                cooldownTimer += Time.deltaTime;
                                getPositionTimer += Time.deltaTime;
                                gameObject.GetComponent<NavMeshAgent>().speed = speed;

                                if (getPositionTimer > 5.0f || Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<NavMeshAgent>().destination) < 1.0f)
                                {
                                    getPositionTimer = 0.0f;
                                    gameObject.GetComponent<NavMeshAgent>().destination = randomMapPositions[Random.Range(0, randomMapPositions.Length)].position;
                                }

                                if (cooldownTimer > 7.5f)
                                {
                                    getPositionTimer = 0.0f;
                                    mState = MovementState.seeking;
                                    cooldownTimer = 0.0f;
                                    gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
                                }
                            }

                            

                            break;
                    }
                    
                }
                if (canAttack)
                {
                    switch (attackType)
                    {
                        case AttackType.trio:
        
                            if (attack1Timer == 0.0f)
                            {
                                gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                                GameObject clone1 = Instantiate(clone, clone1Position.position, Quaternion.identity);
                                clone1.GetComponent<CloneDash>().timeToDash = attack1Cooldown;
                                clone1.GetComponent<CloneDash>().player = player;
                                clone1.GetComponent<CloneDash>().streets = streets;
                                clone1.transform.SetParent(gameObject.transform);

                                GameObject clone2 = Instantiate(clone, clone2Position.position, Quaternion.identity);
                                clone2.GetComponent<CloneDash>().timeToDash = attack1Cooldown * 1.75f;
                                clone2.GetComponent<CloneDash>().player = player;
                                clone2.GetComponent<CloneDash>().streets = streets;
                                clone2.transform.SetParent(gameObject.transform);

                            }
                            attack1Timer += Time.deltaTime;

                            if (attack1Timer > attack1Cooldown * 2.3f && bossDash == false)
                            {
                                proximityAreaAttack.SetActive(true);
                                //Dash del main boss
                                dashDirection = (player.transform.position - gameObject.transform.position);
                                gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position + dashDirection.normalized * 0.2f * dashDirection.magnitude;
                                dashDirection = gameObject.GetComponent<NavMeshAgent>().destination;
                                bossDash = true;
                                brotherBossModel.GetComponent<TrailRenderer>().enabled = true;
                            }

                            if(bossDash == true)
                            {
                                gameObject.GetComponent<NavMeshAgent>().destination = dashDirection;
                                gameObject.GetComponent<NavMeshAgent>().speed = dashSpeed;
                                gameObject.GetComponent<NavMeshAgent>().acceleration = dashAcceleration;
                            }

                            if(bossDash == true && Vector3.Distance(gameObject.transform.position, gameObject.GetComponent<NavMeshAgent>().destination) < 1.0f)
                            {
                                proximityAreaAttack.SetActive(false);
                                gameObject.GetComponent<EnemyHP>().canBeDamaged = true;
                                gameObject.GetComponent<NavMeshAgent>().speed = 0;
                                vulnerabilityTimer += Time.deltaTime;
                                gameObject.GetComponent<CapsuleCollider>().isTrigger = false;

                                if (vulnerabilityTimer > 1.0f)
                                {
                                    gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
                                    gameObject.GetComponent<EnemyHP>().canBeDamaged = false;
                                    vulnerabilityTimer = 0.0f;
                                    bossDash = false;
                                    attack1Timer = 0.0f;
                                    attackSelected = false;
                                    gameObject.GetComponent<NavMeshAgent>().speed = speed;
                                    gameObject.GetComponent<NavMeshAgent>().acceleration = acceleration;
                                    gameObject.GetComponent<NavMeshAgent>().destination = player.transform.position;
                                    cooldownTimer = 0.0f;
                                    canMove = true;
                                    mState = MovementState.hiding;
                                    getPositionTimer = 5.0f;
                                    canAttack = false;
                                    brotherBossModel.GetComponent<TrailRenderer>().enabled = false;

                                }
                            }
                            break;
                        case AttackType.circle:

                            gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;

                            for (int i = 0; i < 10; i++)
                            {
                                // Calcular la posición alrededor del objeto principal
                                float angle = i * (360 / 10);
                                Vector3 position = player.transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * cloneRadius;

                                // Instanciar el objeto en la posición calculada
                                GameObject circleClone = Instantiate(clone, position, Quaternion.identity);
                                circleClone.GetComponent<CloneDash>().timeToDash = attack1Cooldown;
                                circleClone.GetComponent<CloneDash>().player = player;
                                circleClone.GetComponent<CloneDash>().streets = streets;
                            }
                            mState = MovementState.seeking;
                            canAttack = false;
                            canMove = true;
                            cooldownTimer = 0.0f; 
                            attackSelected = false;
                            break;

                        case AttackType.walls:

                            GameObject wall1 = Instantiate(cloneWall, gameObject.transform.position, gameObject.transform.rotation);
                            GameObject wall2 = Instantiate(cloneWall, gameObject.transform.position, gameObject.transform.rotation);

                            wall1.transform.Rotate(0, 10, 0);
                            wall2.transform.Rotate(0, -10, 0);


                            passiveCharge = 0.0f;
                            canAttack = false;
                            attackSelected = false;
                            cooldownTimer = 0.0f;
                            canMove = true;
                            activeWalls = true;
                            break;
                    }
                }

                if (gameObject.GetComponent<EnemyHP>().stun == true)
                {
                    stunTimer += Time.deltaTime;
                    if (stunTimer > 1.5f)
                    {
                        proximityAreaAttack.tag = "Parryable";
                        gameObject.GetComponent<NavMeshAgent>().speed = speed;
                        gameObject.GetComponent<NavMeshAgent>().acceleration = acceleration;
                        gameObject.GetComponent<NavMeshAgent>().destination = gameObject.transform.position;
                        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
                        gameObject.GetComponent<EnemyHP>().canBeDamaged = false;
                        stunTimer = 0.0f;
                        getPositionTimer = 5.0f;
                        gameObject.GetComponent<EnemyHP>().stun = false;
                        mState = MovementState.hiding;
                        canMove = true;
                    }
                    else
                    {
                        brotherBossModel.GetComponent<TrailRenderer>().enabled = false;
                        bossDash = false;
                        vulnerabilityTimer = 0.0f;
                        gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
                        gameObject.GetComponent<EnemyHP>().canBeDamaged = true;
                        gameObject.GetComponent<NavMeshAgent>().destination = gameObject.transform.position;
                        gameObject.GetComponent<NavMeshAgent>().speed = 0.0f;
                        attack1Timer = 0.0f;
                        proximityAreaAttack.SetActive(false);
                        cooldownTimer = 0.0f;
                        attackSelected = false;
                        canAttack = false;
                        canMove = false;
                    }
                }

                break;

            case 1:

                if (canMove)
                {
                    switch (mState)
                    {
                        case MovementState.aerial:
                            if (Vector3.Distance(aerialPosition.position, gameObject.transform.position) < 0.5f)
                            {
                                mState = MovementState.hiding;
                                break;
                            }
                            Vector3 distanceToPoint = aerialPosition.position - gameObject.transform.position;
                            transform.position += distanceToPoint * 3.0f * Time.deltaTime;

                            
                            break;

                        case MovementState.hiding:

                            cooldownTimer += Time.deltaTime;

                            if(cooldownTimer > 15.0f)
                            {
                                canMove = false;
                                cooldownTimer = 0.0f;
                                SelectAttack();
                            }

                            break;
                    }
                }
                if (canAttack)
                {
                    switch (attackType)
                    {
                        case AttackType.disc:

                            for (int i = 0; i < discMovementArea.Length; i++)
                            {
                                GameObject rotatingDisc = Instantiate(disc, gameObject.transform.position, Quaternion.identity);
                                rotatingDisc.GetComponent<AerialDisc>().pointArea = discMovementArea[i];
                            }

                            canAttack = false;
                            canMove = true;
                            break;

                        case AttackType.drones:

                            if(numDrones == 1)
                            {
                                canAttack = false;
                                canMove = true;
                                numDrones = 0;
                                droneTimer = 0.0f;
                                break;
                            }

                            droneTimer += Time.deltaTime;

                            if (droneTimer > dronesCooldown[numDrones])
                            {
                                GameObject droneEnemy = Instantiate(drone, gameObject.transform.position, Quaternion.identity);
                                droneEnemy.GetComponent<Drone>().player = player;
                                droneEnemy.GetComponent<EnemyHP>().hp = 2;
                                numDrones++;
                            }
                            
                            
                            break;

                        case AttackType.car:
                            
                            car.GetComponent<CarAttack>().enabled = true;
                            car.GetComponent<CarAttack>().player = player;
                            car.GetComponent<CarAttack>().bBoss = gameObject.GetComponent<BrotherBoss>();
                            car.GetComponent<NavMeshAgent>().enabled = true;
                            car.tag = "NonParryable";
                            car.layer = 7;
                            car.GetComponent<BoxCollider>().isTrigger = false;
                            for (int i = 0; i < 4; i++)
                            {
                                car.GetComponent<CarAttack>().carWheels[i].GetComponent<CarWheel>().enabled = true;
                            }
                            break;

                        case AttackType.fall:

                            fallTimer += Time.deltaTime;

                            switch (fallState)
                            {
                                
                                case FallAttackState.positioning:
                                    proximityAreaAttack.SetActive(true);

                                    Vector3 distanceToPlayer = player.transform.position - gameObject.transform.position;
                                    distanceToPlayer.y += 6.5f;
                                    transform.position += distanceToPlayer * Time.deltaTime * 4.0f;

                                    if (fallTimer > 5.0f || distanceToPlayer.magnitude < 1.0f)
                                    {
                                        fallTimer = 0.0f;
                                        fallState = FallAttackState.falling;
                                        //Set y to ground
                                    }
                                    break;
                                case FallAttackState.falling:


                                    if (!landed)
                                    {
                                        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -1, 0) * 20, ForceMode.VelocityChange);
                                    }
                                    else
                                    {
                                        Vector3 klk = new Vector3(gameObject.transform.position.x, -127.0f, gameObject.transform.position.z);
                                        GameObject circle = Instantiate(continousCircle, klk, Quaternion.identity);
                                        circle.transform.Rotate(new Vector3(-90, 0, 0));
                                        circle.transform.localScale = circle.transform.localScale / 100.0f;
                                        proximityAreaAttack.SetActive(false);
                                        fallTimer = 0.0f;
                                        fallState = FallAttackState.resting;
                                        gameObject.GetComponent<EnemyHP>().canBeDamaged = true;

                                    }

                                    break;

                                case FallAttackState.resting:
                                    if(fallTimer > 2.5f)
                                    {
                                        gameObject.GetComponent<EnemyHP>().canBeDamaged = false;
                                        fallState = FallAttackState.protecting;
                                        fallTimer = 0.0f;
                                    }

                                    break;


                                case FallAttackState.protecting:

                                    attackSelected = false;
                                    canMove = true;
                                    mState = MovementState.aerial;
                                    fallState = FallAttackState.positioning;
                                    canAttack = false;

                                    break;
                                
                                }

                            break;

                    }
                }

                break;
        }

        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 3 || collision.gameObject.layer == 6)
        {
            landed = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.layer == 3 || collision.gameObject.layer == 6)
        {
            landed = false;
        }
    }
    void SelectAttack()
    {
        switch (phase)
        {
            case 0:

                int value = Random.Range(0, 10);
                if (value < 7) attackType = AttackType.trio;
                else attackType = AttackType.circle;

                if (passiveCharge > 60.0f) attackType = AttackType.walls;

                attackSelected = true;
                break;

            case 1:

                int attack = Random.Range(0, 4);
                switch (attack)
                {
                    case 0:
                        attackType = AttackType.disc;
                        break;
                    case 1:
                        attackType = AttackType.drones;
                        break;
                    case 2:
                        attackType = AttackType.car;
                        break;
                    case 3:
                        attackType = AttackType.fall;
                        break;
                }

                canAttack = true;
                attackSelected = true;
                break;
        }
    }
}
