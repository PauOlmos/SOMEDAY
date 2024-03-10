using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationClip[] animations;
    private Animator animation;

    public PlayerMovement pMov;
    public PlayerAttack pAttack;
    public PassiveAbility pAbility;
    public Parry parry;

    public GameObject sword;
    public float animationsTimer = 0.0f;
    public enum AnimationState
    {
        idle,run,jump,dash,parry,attack,takeDmg,restoreHp,shootProj,chargePassive,die,floating
    }

    public AnimationState animState = AnimationState.idle;

    void Start()
    {
        animation = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
        if (Time.timeScale != 0)
        {
            switch (animState)
            {
                case AnimationState.idle:

                    Debug.Log(animations[0].name);
                    animation.Play(animations[0].name);

                    break;
                case AnimationState.run:

                    Debug.Log(animations[1].name);
                    animation.Play(animations[1].name);

                    break;
                case AnimationState.attack:

                    Debug.Log(animations[2].name);
                    animation.Play(animations[2].name);

                    break;
                case AnimationState.shootProj:
                    animationsTimer += Time.deltaTime;

                    if (animationsTimer >= animations[3].length)
                    {
                        animState = AnimationState.idle;
                        animationsTimer = 0;
                        sword.SetActive(true);

                    }

                    Debug.Log(animations[3].name);
                    animation.Play(animations[3].name);

                    break;
                case AnimationState.jump:
                    animationsTimer += Time.deltaTime;

                    if (animationsTimer >= animations[4].length / 2)
                    {
                        animState = AnimationState.floating;
                        animation.SetBool("JumpToFloat", true);
                        animationsTimer = 0;
                    }

                    Debug.Log(animations[4].name);
                    animation.Play(animations[4].name);

                    break;
                case AnimationState.floating:

                    Debug.Log(animations[5].name);
                    animation.Play(animations[5].name);

                    break;
                case AnimationState.dash:

                    Debug.Log(animations[6].name);
                    animation.Play(animations[6].name);

                    break;

                case AnimationState.takeDmg:

                    animationsTimer += Time.deltaTime;

                    if (animationsTimer >= animations[7].length)
                    {
                        if (pMov.grounded == true) animState = AnimationState.idle;
                        else animState = AnimationState.floating;
                        animationsTimer = 0;
                    }

                    Debug.Log(animations[7].name);
                    animation.Play(animations[7].name);

                    break;
                case AnimationState.die:

                    Debug.Log(animations[8].name);
                    animation.Play(animations[8].name);

                    break;

                case AnimationState.restoreHp:
                    animationsTimer += Time.deltaTime;

                    if (animationsTimer >= animations[9].length)
                    {
                        animState = AnimationState.idle;
                        animationsTimer = 0;
                        sword.SetActive(true);

                    }

                    Debug.Log(animations[9].name);
                    animation.Play(animations[9].name);

                    break;
                case AnimationState.parry:
                    animationsTimer += Time.deltaTime;

                    if (animationsTimer >= animations[10].length)
                    {
                        animState = AnimationState.idle;
                        animationsTimer = 0;
                    }

                    Debug.Log(animations[10].name);
                    animation.Play(animations[10].name);

                    break;

                case AnimationState.chargePassive:
                    Debug.Log(animations[11].name);
                    animation.Play(animations[11].name);

                    break;
            }
        }
        

    }

    private void CheckMovement()
    {
        if(animState != AnimationState.die)
        {
            if (animState != AnimationState.takeDmg)
            {
                if (pAbility.shootNow == true)//Projectile
                {
                    animState = AnimationState.shootProj;
                    sword.SetActive(false);
                    pAbility.shootNow = false;
                }
                if (pAbility.healNow == true)//Projectile
                {
                    animState = AnimationState.restoreHp;
                    sword.SetActive(false);
                    pAbility.healNow = false;
                }
                if (animState != AnimationState.shootProj && animState != AnimationState.jump && animState != AnimationState.restoreHp)
                {
                    if (pMov.grounded == false && pMov.enabled == true)
                    {
                        animState = AnimationState.floating;
                    }
                    else
                    {
                        animation.SetBool("JumpToFloat", false);

                        switch (pMov.pStatus)
                        {
                            case PlayerMovement.playerState.stand:
                                sword.SetActive(true);

                                animState = AnimationState.idle;

                                break;
                            case PlayerMovement.playerState.moving:
                                sword.SetActive(true);

                                animState = AnimationState.run;

                                break;
                            case PlayerMovement.playerState.charging:
                                sword.SetActive(false);
                                animState = AnimationState.chargePassive;

                                break;
                        }



                    }//Movement
                    if (pAttack.attackActive == false)
                    {
                        animState = AnimationState.attack;
                    }
                    if (pMov.pStatus == PlayerMovement.playerState.dashing)
                    {
                        animState = AnimationState.dash;
                    }
                    if (parry.parrying == true)
                    {
                        animState = AnimationState.parry;
                    }
                }//Projectile Priority


            }
        }
        
    }
        
}
