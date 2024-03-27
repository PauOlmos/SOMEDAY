using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadBossAnimations : MonoBehaviour
{
    public enum AnimationsState
    {
        idle, attack, damage
    }

    public AnimationsState animState = AnimationsState.idle;
    public AnimationClip[] animations;
    public AnimationClip actualAnimation;
    public GameObject model;
    public Animator animation;
    public DadBoss boss;
    public bool attacking = false;
    public float damagedTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        animation = model.GetComponent<Animator>();
        animation.Play(animations[0].name);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<DadBoss>() != null)
        {
            boss = gameObject.GetComponent<DadBoss>();
        }
        if (boss != null)
        {
            if (boss.damaged == true)
            {
                damagedTimer += Time.deltaTime;
                if (damagedTimer > animations[2].length)
                {
                    damagedTimer = 0.0f;
                    boss.damaged = false;
                }
                if (actualAnimation != animations[2])
                {
                    animation.Play(animations[2].name);
                    actualAnimation = animations[2];
                }
            }
            else
            {
                if (boss.canAttack == true && actualAnimation != animations[1])
                {
                    Debug.Log("DADATTACK!!!!");
                    animation.Play(animations[1].name);
                    actualAnimation = animations[1];
                }

                if (boss.canAttack == false && actualAnimation != animations[0])
                {
                    animation.Play(animations[0].name);
                    actualAnimation = animations[0];
                }
            }
            

        }
    }
}
