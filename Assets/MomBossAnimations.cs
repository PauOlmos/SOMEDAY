using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomBossAnimations : MonoBehaviour
{
    public AnimationClip[] animations;
    public AnimationClip actualAnimation;
    public GameObject model;
    public Animator animation;
    public MomBoss boss;
    public float damagedTimer = 0.0f;
    public float beginTimer = 0.0f;
    public bool animateNormally = false;
    // Start is called before the first frame update
    void Start()
    {
        animation = model.GetComponent<Animator>();
        animation.Play(animations[0].name);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<MomBoss>() != null && boss == null)
        {
            boss = gameObject.GetComponent<MomBoss>();
        }
        if (boss != null)
        {
            if (animateNormally == false)
            {
                if (actualAnimation == animations[0])
                {
                    animation.Play(animations[1].name);
                    actualAnimation = animations[1];
                }
                if (actualAnimation == animations[1])
                {
                    beginTimer += Time.deltaTime;
                    if (beginTimer > animations[1].length)
                    {
                        animateNormally = true;
                        beginTimer = 0.0f;
                        animation.Play(animations[2].name);
                        actualAnimation = animations[2];
                    }
                }
            }
            else
            {
                if (boss.damaged == true)
                {
                    damagedTimer += Time.deltaTime;
                    if (damagedTimer > animations[4].length)
                    {
                        damagedTimer = 0.0f;
                        boss.damaged = false;
                    }
                    if (actualAnimation != animations[4])
                    {
                        animation.Play(animations[4].name);
                        actualAnimation = animations[4];
                    }
                }
                else
                {
                    if (boss.canAttack == false && actualAnimation != animations[2])
                    {
                        actualAnimation = animations[2];
                        animation.Play(animations[2].name);
                    }
                    if (boss.canAttack == true && actualAnimation != animations[3])
                    {
                        actualAnimation = animations[3];
                        animation.Play(animations[3].name);
                    }
                }
            }
        }
    }
}
