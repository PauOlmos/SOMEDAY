using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighSchoolBossAnimations : MonoBehaviour
{
    // Start is called before the first frame update
    public enum AnimationsState
    {
        idle, forwardWalking, backwardWalking, meleeattack1, meleeattack2, meleeattack3, specialAttack, superAttack, projectileAttack, recieveDamage, stun
    }

    public AnimationsState animState = AnimationsState.idle;
    public AnimationClip[] animations;
    public AnimationClip actualAnimation;
    public GameObject model;
    public Animator animation;
    public HighSchoolBoss boss;
    public bool attacking = false;
    public float damagedTimer = 0.0f;
    void Start()
    {
        animation = model.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<HighSchoolBoss>() != null)
        {
            boss = gameObject.GetComponent<HighSchoolBoss>();
        }
        if (boss != null)
        {
            if (gameObject.GetComponent<EnemyHP>().stun == true)
            {
                if (actualAnimation != animations[10])
                {
                    actualAnimation = animations[10];
                    animation.Play(animations[10].name);
                }
            }
            else
            {
                if (gameObject.GetComponent<HighSchoolBoss>().damaged == true && attacking == false) //TakeDamage Animation Priority
                {
                    if (actualAnimation != animations[9])
                    {
                        actualAnimation = animations[9];
                        animation.Play(animations[9].name);
                    }
                    damagedTimer += Time.deltaTime;
                    if (damagedTimer >= animations[9].length)
                    {
                        gameObject.GetComponent<HighSchoolBoss>().damaged = false;
                        damagedTimer = 0.0f;
                    }
                }
                else
                {
                    if (attacking == false) CheckMovement();
                    CheckAttacks();
                }
            }
        }
    }

    private void CheckAttacks()
    {
        if (boss.MeleeAttackType == HighSchoolBoss.AttackType.one && actualAnimation != animations[3])
        {
            attacking = true;
            animation.Play(animations[3].name);
            actualAnimation = animations[3];

        }
        if (boss.MeleeAttackType == HighSchoolBoss.AttackType.two && actualAnimation != animations[4])
        {
            attacking = true;

            actualAnimation = animations[4];
            animation.Play(animations[4].name);
        }
        if (boss.MeleeAttackType == HighSchoolBoss.AttackType.three && actualAnimation != animations[5])
        {
            attacking = true;

            actualAnimation = animations[5];
            animation.Play(animations[5].name);
        }
        if(boss.MeleeAttackType == HighSchoolBoss.AttackType.special && actualAnimation != animations[6])
        {
            attacking = true;

            actualAnimation = animations[6];
            animation.Play(animations[6].name);
        }
        if(boss.MeleeAttackType == HighSchoolBoss.AttackType.portals && actualAnimation != animations[7])
        {
            attacking = true;

            actualAnimation = animations[7];
            animation.Play(animations[7].name);
        }
        if(boss.MeleeAttackType == HighSchoolBoss.AttackType.shoot && actualAnimation != animations[8])
        {
            attacking = true;

            actualAnimation = animations[8];
            animation.Play(animations[8].name);
        }
        if (boss.MeleeAttackType == HighSchoolBoss.AttackType.reset) attacking = false;
        
    }

    private void CheckMovement()
    {
        if (boss.agent.speed == 0)
        {
            actualAnimation = animations[0];
            animation.Play(animations[0].name);
        }
        if (boss.movState == HighSchoolBoss.MovementState.seeking || boss.movState == HighSchoolBoss.MovementState.toTable)
        {
            actualAnimation = animations[1];
            animation.Play(animations[1].name);
        }
        if(boss.movState == HighSchoolBoss.MovementState.hiding)
        {
            actualAnimation = animations[2];
            animation.Play(animations[2].name);
        }
    }
}
