using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalBoss : MonoBehaviour
{
    public int phase = 1;
    // Start is called before the first frame update
    public GameObject player;
    public bool canAttack = false;

    public float attackTimer = 0.0f;
    
    public enum AttackType
    {
        Projectiles, slash, sword, ray
    }

    public enum SwordAttackStates
    {
        attack, rest, back
    }

    public SwordAttackStates swordAttackStates;
    public float swordAttackTimer = 0.0f;

    public AttackType attackType;

    public int numProjectiles = 0;
    public int numAttacks = 0;
    public GameObject projectiles;

    public Transform projectileSource;

    public float projectilesCooldown = 0.0f;

    public Animator animator;
    public AnimationClip idle;
    public AnimationClip slashAttack;
    public AnimationClip stabAttack;
    int rayType = 0;
    public AnimationClip rayAttack1;
    public AnimationClip rayAttack2;
    public float slashAttackTimer = 0.0f;
    public float rayAttackTimer = 0.0f;
    public GameObject realSword;
    public GameObject auxiliarSword;
    public bool isPlayerOnSword = false;
    public GameObject isOnSword;
    public GameObject swordGround;
    public GameObject rayVisual;
    public GameObject rayCollider;
    public Transform randomPositionsArea;
    public bool isMoving = true;
    public int randomTime = 0;
    public float phase2Timer = 0.0f;
    public bool right = false;
    public int hp = 30;
    public enum Phase2State
    {
        climb, warden
    }

    public Phase2State phase2State = Phase2State.climb;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (phase)
        {
            case 1:
                if (!canAttack)
                {
                    attackTimer += Time.deltaTime;
                    if (attackTimer > 10.0f) SelectAttack();
                }
                else
                {
                    switch (attackType)
                    {
                        case AttackType.Projectiles:

                            projectilesCooldown += Time.deltaTime;

                            if (projectilesCooldown > 0.25f)
                            {
                                GameObject p = Instantiate(projectiles, projectileSource.transform.position, Quaternion.identity);
                                Vector3 forceVector = new Vector3(Random.Range(-100, 100), Random.Range(100, 200), Random.Range(-400, -600));
                                p.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
                                p.GetComponent<FinalBossProjectile>().player = player;
                                numProjectiles++;
                                projectilesCooldown = 0.0f;
                            }

                            if (numProjectiles > 20)
                            {
                                projectilesCooldown = 0;
                                numProjectiles = 0;
                                canAttack = false;
                            }

                            break;

                        case AttackType.slash:

                            slashAttackTimer += Time.deltaTime;

                            if (slashAttackTimer > 36.0f && slashAttackTimer < 42.5f)
                            {
                                realSword.SetActive(false);
                                auxiliarSword.SetActive(true);
                                auxiliarSword.transform.Rotate(Vector3.left * Time.deltaTime * 25.0f);

                            }

                            if (slashAttackTimer > 42.5f)
                            {
                                auxiliarSword.SetActive(false);
                                realSword.SetActive(true);

                            }

                            if (slashAttackTimer * 0.05f > slashAttack.length)
                            {
                                slashAttackTimer = 0.0f;
                                animator.Play(idle.name);
                                canAttack = false;
                                Vector3 currentRotation = auxiliarSword.transform.rotation.eulerAngles;
                                currentRotation.y = 180f;
                                auxiliarSword.transform.rotation = Quaternion.Euler(currentRotation);
                                auxiliarSword.transform.Rotate(180, 0, 0);
                            }

                            break;

                        case AttackType.ray:

                            rayAttackTimer += Time.deltaTime;

                            if (rayAttackTimer > 5.0f) rayCollider.SetActive(true);
                            if (rayType == 0)
                            {
                                if (rayAttackTimer * 0.2f > rayAttack1.length)
                                {
                                    rayAttackTimer = 0.0f;
                                    animator.Play(idle.name);
                                    canAttack = false;
                                    rayVisual.SetActive(false);
                                    rayCollider.SetActive(false);
                                }
                            }
                            else
                            {
                                if (rayAttackTimer * 0.2f > rayAttack2.length)
                                {
                                    rayAttackTimer = 0.0f;
                                    animator.Play(idle.name);
                                    canAttack = false;
                                    rayVisual.SetActive(false);
                                    rayCollider.SetActive(false);
                                }
                            }



                            break;

                        case AttackType.sword:

                            switch (swordAttackStates)
                            {
                                case SwordAttackStates.attack:

                                    swordAttackTimer += Time.deltaTime;
                                    if (swordAttackTimer * 0.05f > stabAttack.length)
                                    {
                                        swordAttackTimer = 0.0f;
                                        swordAttackStates = SwordAttackStates.rest;
                                        isOnSword.SetActive(true);
                                    }

                                    break;

                                case SwordAttackStates.rest:

                                    swordAttackTimer += Time.deltaTime;

                                    if (swordAttackTimer > 18.0f && isOnSword.GetComponent<IsPlayerOnSword>().isOnSwordThePlayer == false)
                                    {
                                        swordAttackTimer = 0.0f;
                                        animator.SetBool("StabToIdle", true);

                                        swordAttackStates = SwordAttackStates.back;
                                        isOnSword.SetActive(false);
                                        swordGround.GetComponent<MeshCollider>().isTrigger = true;

                                    }

                                    //if(isOnSword.GetComponent<IsPlayerOnSword>().isOnSwordThePlayer == true) animator.SetBool("StabToIdle", false);

                                    break;

                                case SwordAttackStates.back:

                                    swordAttackTimer += Time.deltaTime;

                                    if (swordAttackTimer > 20.0f)
                                    {
                                        swordAttackTimer = 0.0f;
                                        animator.SetBool("StabToIdle", false);
                                        swordGround.GetComponent<MeshCollider>().isTrigger = false;
                                        animator.Play(idle.name);
                                        canAttack = false;
                                    }

                                    break;

                                default: break;

                            }

                            break;

                    }




                }
                break;
            case 2:

                switch (phase2State)
                {
                    case Phase2State.climb:

                        if(gameObject.transform.position.y > -216.1)
                        {
                            gameObject.transform.position = new Vector3(gameObject.transform.position.x, -216.1f, gameObject.transform.position.z);
                            phase2State = Phase2State.warden;
                            randomTime = 120;
                            //ChangeTo Warden
                        }
                        else
                        {
                            gameObject.transform.Translate(Vector3.up * Time.deltaTime * 10.0f);
                            gameObject.transform.Translate(-Vector3.forward * Time.deltaTime * 10.0f);
                        }

                        break;
                    case Phase2State.warden:

                        if (isMoving)
                        {
                            phase2Timer += Time.deltaTime;
                            gameObject.transform.Translate(-gameObject.transform.forward * Time.deltaTime);
                            if(phase2Timer > randomTime)
                            {
                                phase2Timer = 0.0f;
                                randomTime = 7;
                                isMoving = false;
                            }
                        }
                        else
                        {
                            phase2Timer += Time.deltaTime;
                            if(right) gameObject.transform.Rotate(Vector3.up * Time.deltaTime * 2.0f);
                            else gameObject.transform.Rotate(-Vector3.up * Time.deltaTime * 2.0f);
                            if (phase2Timer > randomTime)
                            {
                                isMoving = true;
                                phase2Timer = 0.0f;
                                randomTime = 30;
                                right = true;
                            }
                        }

                        break;
                }

                break;
        }
        
    }

    public void SelectAttack()
    {
        numAttacks++;
        int value = Random.Range(0, 4);
        if (numAttacks > 7) value = 4;
        switch (value)
        {
            case 0:
            case 1:
                attackType = AttackType.Projectiles;
                break;
            case 2:
                attackType = AttackType.slash;
                animator.Play(slashAttack.name);
                break;

            case 3:
                rayType = Random.Range(0, 2);
                attackType = AttackType.ray;
                if(rayType == 0)animator.Play(rayAttack1.name);
                else animator.Play(rayAttack2.name);
                rayVisual.SetActive(true);

                break;

            case 4://Fer passiva i canviar per rayo de la muerte
                numAttacks = 4;
                attackType = AttackType.sword;
                swordAttackStates = SwordAttackStates.attack;
                animator.Play(stabAttack.name);

                break;
        }
        canAttack = true;
        attackTimer = 0.0f;
    }
}
