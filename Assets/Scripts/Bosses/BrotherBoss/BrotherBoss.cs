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
        trio, circle, walls
    }

    public AttackType attackType;

    public bool attackSelected = false;

    public GameObject player;
    public GameObject proximityAreaAttack;

    public Transform clone1Position;
    public Transform clone2Position;

    public GameObject clone;

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


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().speed = speed;
        proximityAreaAttack.transform.position = Vector3.zero;
        proximityAreaAttack.transform.localScale = Vector3.one * 2;
        proximityAreaAttack.transform.SetParent(brotherBossModel.transform);
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        proximityAreaAttack.transform.localPosition = Vector3.zero;

        switch (phase)
        {
            case 0:
                if (canMove)
                {
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
                        if (attackType == AttackType.circle)
                        {
                            canMove = false;
                            canAttack = false;
                        }
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
                                Debug.Log("CreateClonesPlz");
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
                                    gameObject.GetComponent<NavMeshAgent>().destination = gameObject.transform.position;
                                    cooldownTimer = 0.0f;
                                    canMove = true;
                                    canAttack = false;
                                }
                            }
                            break;
                        case AttackType.circle:
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
                        gameObject.GetComponent<EnemyHP>().stun = false;
                        canMove = true;
                    }
                    else
                    {
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
        }

        
    }

    void SelectAttack()
    {
        switch (phase)
        {
            case 0:

                int value = Random.Range(0, 10);
                value = 1;
                if (value < 7) attackType = AttackType.trio;
                else attackType = AttackType.circle;
                attackSelected = true;
                break;
        }
    }
}
