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
                    if(pMov.grounded == true) animState = AnimationState.idle;
                    else animState = AnimationState.floating;
                    animationsTimer = 0;
                }

                Debug.Log(animations[7].name);
                animation.Play(animations[7].name);

                break;

        }

    }

    private void CheckMovement()
    {
        if(animState != AnimationState.takeDmg)
        {
            if (pAbility.shootNow == true)//Projectile
            {
                animState = AnimationState.shootProj;
                sword.SetActive(false);
                pAbility.shootNow = false;
            }
            if (animState != AnimationState.shootProj && animState != AnimationState.jump)
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

                            animState = AnimationState.idle;

                            break;
                        case PlayerMovement.playerState.moving:

                            animState = AnimationState.run;

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
            }//Projectile Priority


        }
    }
        
}
