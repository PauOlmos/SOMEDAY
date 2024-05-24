using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrotherBossAnimations : MonoBehaviour
{
    public enum AnimationsState
    {
        driving, idle, run, dash, specialAttack, recieveDamage, stun, circleAttack, 
    }

    public AnimationsState animState = AnimationsState.idle;
    public AnimationClip[] animations;
    public AnimationClip actualAnimation;
    public GameObject model;
    public Animator animation;
    public BrotherBoss boss;
    public StartBrotherBoss startBoss;
    public bool attacking = false;
    public float damagedTimer = 0.0f;
    public float phase2AttackTimer = 0.0f;
    public bool didAttackLand = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<BrotherBoss>() != null)
        {
            boss = gameObject.GetComponent<BrotherBoss>();
        }
        if (startBoss != null)
        {
            if(startBoss.transitionState ==  StartBrotherBoss.TransitionState.driving|| startBoss.transitionState == StartBrotherBoss.TransitionState.waiting)
            {
                if (actualAnimation != animations[1])
                {
                    actualAnimation = animations[1];
                    animation.Play(animations[1].name);
                }
            }
        }
        else
        {
            if (boss != null)
            {
                switch (boss.phase)
                {
                    case 0:

                        if (boss.canMove == true)
                        {
                            if (actualAnimation != animations[2])
                            {
                                damagedTimer = 0.0f;
                                gameObject.GetComponent<BrotherBoss>().damaged = false;
                                actualAnimation = animations[2];
                                animation.Play(animations[2].name);
                            }
                        }
                        else
                        {
                            if (gameObject.GetComponent<EnemyHP>().stun == true)
                            {
                                if (actualAnimation != animations[6])
                                {
                                    actualAnimation = animations[6];
                                    animation.Play(animations[6].name);
                                }
                            }
                            else
                            {
                                if (gameObject.GetComponent<BrotherBoss>().damaged == true) //TakeDamage Animation Priority
                                {
                                    if (actualAnimation != animations[5])
                                    {
                                        actualAnimation = animations[5];
                                        animation.Play(animations[5].name);
                                    }
                                    damagedTimer += Time.deltaTime;
                                    if (damagedTimer >= animations[5].length)
                                    {
                                        gameObject.GetComponent<BrotherBoss>().damaged = false;
                                        damagedTimer = 0.0f;
                                    }
                                }
                                else
                                {
                                    if (boss.canAttack == true)
                                    {
                                        if (boss.bossDash == false)
                                        {
                                            if (actualAnimation != animations[3])
                                            {
                                                actualAnimation = animations[3];
                                                animation.Play(animations[3].name);
                                            }
                                        }
                                        else
                                        {
                                            if (actualAnimation != animations[4])
                                            {
                                                actualAnimation = animations[4];
                                                animation.Play(animations[4].name);
                                            }
                                        }

                                    }
                                }
                            }



                        }

                        break;
                    case 1:
                        if (gameObject.GetComponent<BrotherBoss>().damaged == true) //TakeDamage Animation Priority
                        {
                            if (actualAnimation != animations[5])
                            {
                                actualAnimation = animations[5];
                                animation.Play(animations[5].name);
                            }
                            damagedTimer += Time.deltaTime;
                            if (damagedTimer >= animations[5].length)
                            {
                                gameObject.GetComponent<BrotherBoss>().damaged = false;
                                damagedTimer = 0.0f;
                            }
                        }
                        else
                        {
                            if (attacking == true)
                            {
                                if (actualAnimation != animations[8])
                                {
                                    actualAnimation = animations[8];
                                    animation.Play(animations[8].name);
                                }
                                phase2AttackTimer += Time.deltaTime;
                                if (phase2AttackTimer >= animations[8].length)
                                {
                                    attacking = false;
                                    phase2AttackTimer = 0.0f;
                                }
                            }
                            else
                            {
                                if (boss.canMove)
                                {
                                    didAttackLand = false;

                                    if (actualAnimation != animations[0])
                                    {
                                        actualAnimation = animations[0];
                                        animation.Play(animations[0].name);
                                    }
                                }
                                else
                                {
                                    if (boss.canAttack)
                                    {
                                        if (boss.attackType == BrotherBoss.AttackType.fall)
                                        {

                                            if (boss.fallState != BrotherBoss.FallAttackState.positioning)
                                            {
                                                if (actualAnimation != animations[7] && didAttackLand == false)
                                                {
                                                    didAttackLand = true;
                                                    actualAnimation = animations[7];
                                                    animation.Play(animations[7].name);
                                                }
                                            }
                                            else
                                            {
                                                if (actualAnimation != animations[0])
                                                {
                                                    actualAnimation = animations[0];
                                                    animation.Play(animations[0].name);
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        break;
                }
            }
            
        }
        
    }
}
